using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class MatchViewModel : ViewModelBase
{
    public MatchViewModel(Match match)
    {
        Match = match;
        Match.PropertyChanged += (sender, args) => UpdateMatch();
        UpdateMatch();
    }
    
    [ObservableProperty]
    private Match _match;

    [ObservableProperty] 
    private bool _notStarted;
    
    [ObservableProperty]
    private bool _readyToStart;
    
    [ObservableProperty]
    private bool _inProgress;
    
    [ObservableProperty]
    private bool _finished;
    
    public void UpdateMatch()
    {
        NotStarted = Match.State == Match.MatchState.NotStarted;
        ReadyToStart = Match.State == Match.MatchState.ReadyToStart;
        InProgress = Match.State == Match.MatchState.InProgress;
        Finished = Match.State == Match.MatchState.Finished;
    }

    [RelayCommand]
    private void StartMatch()
    {
        if (Match.State == Match.MatchState.ReadyToStart)
        {
            Match.StartMatch();
        }
    }

    [RelayCommand]
    private void EndMatch(Team winner)
    {
        if (Match.State == Match.MatchState.InProgress)
        {
            Match.EndMatch(winner == Match.Team1);
        }
    }
}