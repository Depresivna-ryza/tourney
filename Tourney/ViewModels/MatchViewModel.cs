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
    
    [ObservableProperty]
    private bool _winner1;
    
    [ObservableProperty]
    private bool _winner2;
    
    
    [ObservableProperty]
    private string _color1 = "#cccccc";
    
    [ObservableProperty]
    private string _color2 = "#cccccc";
    
    
    public void UpdateMatch()
    {
        NotStarted = Match.State == Match.MatchState.NotStarted;
        ReadyToStart = Match.State == Match.MatchState.ReadyToStart;
        InProgress = Match.State == Match.MatchState.InProgress;
        
        if (Match.State == Match.MatchState.Finished)
        {
            Finished = true;
            Winner1 = Match.Winner == Match.Team1;
            Winner2 = Match.Winner == Match.Team2;
            
            Color1 = Winner1 ? "White" : "#888888";
            Color2 = Winner2 ? "White" : "#888888";
        }
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