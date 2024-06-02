using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    
    public ObservableCollection<AbstractTourney> Tourneys { get; set; }
    
    [ObservableProperty]
    private AbstractTourney? _selectedTourney;

    [ObservableProperty] 
    private EliminationTourneyViewModel? _eliminationTourneyViewModel;    
    
    [ObservableProperty] 
    private VersusTourneyViewModel? _versusTourneyViewModel;    
    
    [ObservableProperty]
    private bool _isEliminationTourney;
    
    [ObservableProperty]
    private bool _isVersusTourney;
    
    [ObservableProperty] 
    private bool _enabled = false;

    
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
        
        if (SelectedTourney is VersusTourney)
        {
            IsVersusTourney = true;
            IsEliminationTourney = false;
            VersusTourneyViewModel = new VersusTourneyViewModel(SelectedTourney as VersusTourney);
        }
        else if (SelectedTourney is EliminationTourney)
        {
            IsEliminationTourney = true;
            IsVersusTourney = false;
            EliminationTourneyViewModel = new EliminationTourneyViewModel(SelectedTourney as EliminationTourney);
        }
        
        // TourneyViewModel.UpdateTourney(SelectedTourney);
    }

    private void UpdateTourneys()
    {
        Tourneys = TourneyManager.Instance.Tourneys;
    }



}