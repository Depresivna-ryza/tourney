using System;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;

public partial class EliminationMatch : ObservableObject
{
    public EliminationMatch(EliminationMatch nextEliminationMatch, bool nextMatchSlot1)
    {
        NextEliminationMatch = nextEliminationMatch;
        NextMatchSlot1 = nextMatchSlot1;
        IsFinal = false;
    }
    
    public EliminationMatch()
    {
        IsFinal = true;
    }
    
    [ObservableProperty]
    private Team? _team1 = null;
    [ObservableProperty]
    private Team? _team2 = null;

    [ObservableProperty] 
    private Team? _winner = null;

    [ObservableProperty] 
    private TimeSpan _duration = TimeSpan.Zero;
    
    [ObservableProperty]
    private EliminationMatch? _nextEliminationMatch = null;
    
    [ObservableProperty]
    private bool _nextMatchSlot1;


    [ObservableProperty] 
    private bool _isFinal;
    
    [ObservableProperty]
    private MatchState _state = MatchState.NotStarted;
    
    
    public enum MatchState
    {
        NotStarted,
        ReadyToStart,
        InProgress,
        Finished
    }

    public void AddTeam(Team team, bool slot1)
    {
        if (slot1)
        {
            Team1 = team;
        }
        else
        {
            Team2 = team;
        }
        
        if (Team1 != null && Team2 != null)
        {
            State = MatchState.ReadyToStart;
        }
    }

    public void StartWithoutOpponent()
    {
        State = MatchState.Finished;
        Duration = TimeSpan.Zero;
        Winner = Team1 ?? Team2;
        
        if (NextEliminationMatch != null)
        {
            NextEliminationMatch.AddTeam(Winner!, NextMatchSlot1);
        }
    }
    

    public void StartMatch()
    {
        State = MatchState.InProgress;
    }
    
    public void EndMatch(bool slot1, TimeSpan duration)
    {
        Winner = slot1 ? Team1 : Team2;
        State = MatchState.Finished;
        Duration = duration;
        
        if (NextEliminationMatch != null)
        {
            NextEliminationMatch.AddTeam(Winner!, NextMatchSlot1);
        }
    }

}