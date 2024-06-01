using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;


public partial class EliminationTourney : AbstractTourney
{
    public ObservableCollection<ObservableCollection<EliminationMatch>> Rounds { get; set; }
    
    public EliminationTourney(string name, IEnumerable<Team> teams)
    {
        Name = name;
        StartDate = DateTime.Now;
        
        int rounds = (int)Math.Ceiling(Math.Log2(teams.Count()));
        
        Rounds = new ObservableCollection<ObservableCollection<EliminationMatch>>();
        
        for (int i = 0; i < rounds; i++)
        {
            Rounds.Add(new ObservableCollection<EliminationMatch>());
        }
        
        
        for (int i = Rounds.Count - 1; i >= 0; i--)
        {
            if (i == rounds - 1)
            {
                Rounds[i].Add(new EliminationMatch());
            }
            else
            {
                foreach (var match in Rounds[i + 1])
                {
                    Rounds[i].Add(new EliminationMatch(match, true));
                    Rounds[i].Add(new EliminationMatch(match, false));
                }
            }
        }
        
        // occupy round 1 with teams
        for (int i = 0; i < teams.Count(); i++)
        {
            var round = Rounds[0];
            round[i % round.Count].AddTeam(teams.ElementAt(i), i / round.Count == 0);
        }
        
        // conclude matches without an opponent
        foreach (var match in Rounds[0])
        {
            if (match.State == EliminationMatch.MatchState.NotStarted)
            {
                match.StartWithoutOpponent();
            }

        }

        
        // event propagation
        foreach (var round in Rounds)
        {
            foreach (var match in round)
            {
                match.PropertyChanged += (sender, args) => OnPropertyChanged();
            }
            round.CollectionChanged += (sender, args) => OnPropertyChanged();
        }
        
        PropertyChanged += (sender, args) => UpdateWinner();
        
        
    }
    public void UpdateWinner()
    {
        Winner = Rounds[^1][0].Winner;
    }

}