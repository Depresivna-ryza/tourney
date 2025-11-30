using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;

public partial class VersusTourney : AbstractTourney
{
    public VersusTourney(string name, int rounds, Team t1, Team t2)
    {
        Name = name;
        StartDate = DateTime.Now;
        TotalRounds = (rounds / 2) * 2 + 1;
        Team1 = t1;
        Team2 = t2;
    }
    
    [ObservableProperty]
    private Team _team1;
    
    [ObservableProperty]
    private Team _team2;

    [ObservableProperty] 
    private ObservableCollection<bool> _rounds = new();

    [ObservableProperty] 
    private int _currentRound = 0;
    
    [ObservableProperty]
    private int _totalRounds;

    public int Score1()
    {
        return Rounds.Count(x => x);
    }
    
    public int Score2()
    {
        return Rounds.Count(x => !x);
    }
    
    
    public void ConcludeRound(Team team)
    {
        if (team == Team1)
        {
            ConcludeRound(true);
        } else if (team == Team2)
        {
            ConcludeRound(false);
        }
        
    }
    public void ConcludeRound(bool team1)
    {
        Rounds.Add(team1);
        CurrentRound++;
        
        if (CurrentRound == TotalRounds ||
            Score1() > TotalRounds / 2 ||
            Score2() > TotalRounds / 2)
        {
            ConcludeTourney();
        }
    } 
    
    public void ConcludeTourney()
    {
        Winner = Score1() > TotalRounds / 2 ? Team1 : Team2;
    }
}