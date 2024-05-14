using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class TourneysPageViewModel : ViewModelBase
{
    public TourneysPageViewModel()
    {
        Tourneys = TourneyManager.Instance.Tourneys;
        _selectedTourney = Tourneys.Count > 0 ? Tourneys[0] : null;
        
        TourneyManager.Instance.Tourneys.CollectionChanged += (sender, args) => UpdateTourneys();
    }
    
    partial void OnSelectedTourneyChanged (Models.Tourney? oldValue, Models.Tourney? newValue)
    {
        UpdateSelectedTourney();
    }
    
    private void UpdateSelectedTourney()
    {
        TourneyViewModel.UpdateTourney(SelectedTourney);
    }

    private void UpdateTourneys()
    {
        Tourneys = TourneyManager.Instance.Tourneys;
    }

    public ObservableCollection<Models.Tourney> Tourneys { get; set; }
    
    [ObservableProperty]
    private Models.Tourney? _selectedTourney;

    [ObservableProperty] 
    private TourneyViewModel _tourneyViewModel = new TourneyViewModel(null);

}