using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;

public partial class Match : ObservableObject
{
    public Match(Match nextMatch, bool nextMatchSlot1)
    {
        NextMatch = nextMatch;
        NextMatchSlot1 = nextMatchSlot1;
        IsFinal = false;
    }
    
    public Match()
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
    private Match? _nextMatch = null;
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
    
    public void StartMatch()
    {
        State = MatchState.InProgress;
    }
    
    public void EndMatch(bool slot1)
    {
        Winner = slot1 ? Team1 : Team2;
        State = MatchState.Finished;
        if (NextMatch != null)
        {
            NextMatch.AddTeam(Winner!, NextMatchSlot1);
        }
    }

}