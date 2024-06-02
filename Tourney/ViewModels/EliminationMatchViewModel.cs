using System;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class EliminationMatchViewModel : ViewModelBase
{
    public EliminationMatchViewModel(EliminationMatch eliminationMatch)
    {
        EliminationMatch = eliminationMatch;
        EliminationMatch.PropertyChanged += (sender, args) => UpdateMatch();
        UpdateMatch();
        
        Timer.Tick += TimerTick;
    }
    
    [ObservableProperty]
    private EliminationMatch _eliminationMatch;

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
    
    [ObservableProperty]
    private string _logoColor1 = "#cccccc";
    
    [ObservableProperty]
    private string _logoColor2 = "#cccccc";

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
        NotStarted = EliminationMatch.State == EliminationMatch.MatchState.NotStarted;
        ReadyToStart = EliminationMatch.State == EliminationMatch.MatchState.ReadyToStart;
        InProgress = EliminationMatch.State == EliminationMatch.MatchState.InProgress;
        
        if (EliminationMatch.State == EliminationMatch.MatchState.Finished)
        {
            Finished = true;
            Winner1 = EliminationMatch.Winner == EliminationMatch.Team1;
            Winner2 = EliminationMatch.Winner == EliminationMatch.Team2;
            
            Color1 = Winner1 ? "White" : "#666666";
            Color2 = Winner2 ? "White" : "#666666";
            
            VsColor = "#666666";
            
        }
        
        if (EliminationMatch.State == EliminationMatch.MatchState.InProgress)
        {
            Color1 = "White";
            Color2 = "White";
            VsColor = "White";
        }
        
        LogoColor1 = Winner2 ? "#666666" : EliminationMatch.Team1?.Color ?? "#666666";
        LogoColor2 = Winner1 ? "#666666" : EliminationMatch.Team2?.Color ?? "#666666";
    }

    [RelayCommand]
    private void StartMatch()
    {
        if (EliminationMatch.State == EliminationMatch.MatchState.ReadyToStart)
        {
            EliminationMatch.StartMatch();
            
            Timer.Start();
            ElapsedTime = TimeSpan.Zero;
        }
    }

    [RelayCommand]
    private void EndMatch(Team winner)
    {
        if (EliminationMatch.State == EliminationMatch.MatchState.InProgress)
        {
            Timer.Stop();
            EliminationMatch.EndMatch(winner == EliminationMatch.Team1, ElapsedTime);
        }
    }
}