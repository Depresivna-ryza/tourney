using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class OngoingTourneyViewModel : ViewModelBase
{
    public OngoingTourneyViewModel()
    {
        _tourney = TourneyManager.Instance.CurrentTourney;
        TourneyManager.Instance.CurrentTourneyChanged += (sender, args) => UpdateTourney();
    }
    
    public event EventHandler TourneyEnded;
    
    [ObservableProperty] 
    private AbstractTourney _tourney;

    [ObservableProperty] 
    private bool _isFinished;
    
    [ObservableProperty]
    private EliminationTourneyViewModel _eliminationTourneyViewModel = new(null);
    
    [ObservableProperty]
    private VersusTourneyViewModel _versusTourneyViewModel = new(null);

    [ObservableProperty] 
    private bool _isVersusTourney;
    
    [ObservableProperty]
    private bool _isEliminationTourney;
    
    
    public void StartTourney()
    {
        UpdateTourney();
    }
    
    private void UpdateTourney()
    {
        
        var newTourney = TourneyManager.Instance.CurrentTourney;
        
        if (newTourney != null) 
        {
            IsFinished = newTourney.Winner != null;
        }
        
        // if ( newTourney == Tourney)
        // {
        //     return;
        // }
        
        Tourney = newTourney;

        if (Tourney is VersusTourney)
        {
            VersusTourneyViewModel.UpdateTourney(Tourney as VersusTourney);
            IsVersusTourney = true;
            IsEliminationTourney = false;
        }
        else if (Tourney is EliminationTourney)
        {
            EliminationTourneyViewModel.UpdateTourney(Tourney as EliminationTourney);
            IsEliminationTourney = true;
            IsVersusTourney = false;
        }
    }
    
    [RelayCommand]
    private void DiscardTourney()
    {
        TourneyManager.Instance.DiscardCurrentTourney();
        TourneyEnded?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    public void SaveTourney()
    {
        if (TourneyManager.Instance.SaveCurrentTourney()){
            TourneyEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}