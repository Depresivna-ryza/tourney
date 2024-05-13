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
        _ongoingTourneyViewModel = new OngoingTourneyViewModel();
        
        // subscribe to model changes
        TourneyManager.Instance.Tourneys.CollectionChanged += (sender, args) => UpdateTourneys();
    }

    private void UpdateTourneys()
    {
        Tourneys = TourneyManager.Instance.Tourneys;
    }

    public ObservableCollection<Models.Tourney> Tourneys { get; set; }
    
    [ObservableProperty]
    private Models.Tourney? _selectedTourney;
    
    [ObservableProperty]
    private OngoingTourneyViewModel _ongoingTourneyViewModel;
    
}