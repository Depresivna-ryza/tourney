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
    private Models.Tourney _tourney;
    
    [ObservableProperty]
    private TourneyViewModel _tourneyViewModel = new(null);
    
    public void StartTourney()
    {
        UpdateTourney();
    }
    
    private void UpdateTourney()
    {
        Tourney = TourneyManager.Instance.CurrentTourney;
        TourneyViewModel.UpdateTourney(Tourney);
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