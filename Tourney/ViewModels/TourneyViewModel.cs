using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class TourneyViewModel : ViewModelBase
{
    
    public TourneyViewModel(Models.Tourney tourney) {
        UpdateTourney(tourney);
    }

    public ObservableCollection<MatchViewModel> Round1 { get; set; } = new();
    public ObservableCollection<MatchViewModel> Round2 { get; set; } = new();
    public ObservableCollection<MatchViewModel> Round3 { get; set; } = new();
    
    [ObservableProperty] 
    private Models.Tourney _tourney;
    
    private void UpdateTourney(Models.Tourney? newTourney)
    {
        Round1.Clear();
        Round2.Clear();
        Round3.Clear();
        
        if (newTourney == null)
        {
            return;
        }

        Tourney = newTourney;
        
        
        foreach (var match in Tourney.Round1)
        {
            Round1.Add(new MatchViewModel(match));
        }
        
        foreach (var match in Tourney.Round2)
        {
            Round2.Add(new MatchViewModel(match));
        }
        
        foreach (var match in Tourney.Round3)
        {
            Round3.Add(new MatchViewModel(match));
        }
    }

    
}