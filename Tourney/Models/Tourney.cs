using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;


// 8 team single elimination tournament of 3 rounds
public partial class Tourney : ObservableObject
{
    [ObservableProperty] 
    private string _name;
    
    [ObservableProperty]
    private Team? _winner;
    
    public ObservableCollection<Team> Teams { get; set; }
    public ObservableCollection<Match> Round1 { get; set; }
    public ObservableCollection<Match> Round2 { get; set; }
    public ObservableCollection<Match> Round3 { get; set; }
    
    public Tourney(string name, IEnumerable<Team> teams)
    {
        Name = name;
        Teams = new ObservableCollection<Team>();
        
        if (teams.Count() != 8)
        {
            throw new ArgumentException("Exactly 8 teams are required for a tournament", nameof(teams));
        }
        
        foreach (var team in teams)
        {
            Teams.Add(team);
        }
        
        Round3 = new ObservableCollection<Match> { new Match() };
        
        Round2 = new ObservableCollection<Match>();
        foreach (var match in Round3)
        {
            Round2.Add(new Match(match, true));
            Round2.Add(new Match(match, false));
        }
        
        Round1 = new ObservableCollection<Match>();
        foreach (var match in Round2)
        {
            Round1.Add(new Match(match, true));
            Round1.Add(new Match(match, false));
        }
        
        // occupy round 1 with teams
        for (int i = 0; i < 8; i += 2)
        {
            Round1[i/2].AddTeam(Teams[i], true);
            Round1[i/2].AddTeam(Teams[i + 1], false);
        }
    }
}