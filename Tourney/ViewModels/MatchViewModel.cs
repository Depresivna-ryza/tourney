using System;
using Avalonia.Threading;
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
        
        Timer.Tick += TimerTick;
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
    
    [ObservableProperty]
    private string _vsColor = "#666666";

    [ObservableProperty] private DispatcherTimer _timer =
        new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

    [ObservableProperty] 
    private TimeSpan _elapsedTime = TimeSpan.Zero;
    
    private void TimerTick(object? sender, EventArgs e)
    {
        ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
    }
    
    
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
            
            Color1 = Winner1 ? "White" : "#666666";
            Color2 = Winner2 ? "White" : "#666666";
            
            VsColor = "#666666";
        }
        
        if (Match.State == Match.MatchState.InProgress)
        {
            Color1 = "White";
            Color2 = "White";
            VsColor = "White";
        }
    }

    [RelayCommand]
    private void StartMatch()
    {
        if (Match.State == Match.MatchState.ReadyToStart)
        {
            Match.StartMatch();
            
            Timer.Start();
            ElapsedTime = TimeSpan.Zero;
        }
    }

    [RelayCommand]
    private void EndMatch(Team winner)
    {
        if (Match.State == Match.MatchState.InProgress)
        {
            Timer.Stop();
            Match.EndMatch(winner == Match.Team1, ElapsedTime);
        }
    }
}