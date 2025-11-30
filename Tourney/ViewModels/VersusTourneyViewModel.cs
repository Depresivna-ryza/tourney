using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class VersusTourneyViewModel : ViewModelBase
{
    public VersusTourneyViewModel(VersusTourney? tourney)
    {
        VersusTourney = tourney;
        if (VersusTourney != null)
        {
            VersusTourney.PropertyChanged += (sender, args) => UpdateTourney(tourney);
        }
        UpdateTourney(tourney);
    }
    
    [ObservableProperty]
    private VersusTourney? _versusTourney;
    
    [ObservableProperty]
    private ObservableCollection<Tuple<bool,bool>> _rounds = new();

    [ObservableProperty] 
    private bool _winner1;
    
    [ObservableProperty]
    private bool _winner2;
    
    [ObservableProperty]
    private bool _isFinished;
    
    [ObservableProperty]
    private int _score1;
    
    [ObservableProperty]
    private int _score2;
    
    [ObservableProperty]
    private string _scoreString= "0 : 0";
    
    
    [RelayCommand]
    private void WinRound(Team team)
    {
        VersusTourney!.ConcludeRound(team);
    }

    
    public void UpdateTourney(VersusTourney? newTourney)
    {
        
        if (newTourney == null)
        {
            return;
        }
        
        VersusTourney = newTourney;

        Winner1 = VersusTourney.Winner == VersusTourney.Team1;
        Winner2 = VersusTourney.Winner == VersusTourney.Team2;
        IsFinished = VersusTourney.Winner != null;

        Score1 = VersusTourney.Score1();
        Score2 = VersusTourney.Score2();
        
        ScoreString = Score1 + " : " + Score2;
        
        Rounds.Clear();
        foreach (var round in VersusTourney.Rounds)
        {
            Rounds.Add(new Tuple<bool, bool>(round, !round));
        }
    }
}