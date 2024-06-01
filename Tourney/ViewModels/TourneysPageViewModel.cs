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
    
    partial void OnSelectedTourneyChanged(AbstractTourney? oldValue, AbstractTourney? newValue)
    {
        UpdateSelectedTourney();
    }
    
    private void UpdateSelectedTourney()
    {
        if (SelectedTourney == null)
        {
            Enabled = false;
            return;
        }
        
        Enabled = true;
        
        TourneyViewModel = new TourneyViewModel( SelectedTourney as EliminationTourney);
        // TourneyViewModel.UpdateTourney(SelectedTourney);
    }

    private void UpdateTourneys()
    {
        Tourneys = TourneyManager.Instance.Tourneys;
    }

    public ObservableCollection<AbstractTourney> Tourneys { get; set; }
    
    [ObservableProperty]
    private AbstractTourney? _selectedTourney;

    [ObservableProperty] 
    private TourneyViewModel? _tourneyViewModel = null;    
    
    [ObservableProperty] 
    private bool _enabled = false;

}