using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class NewTourneyViewModel : ViewModelBase
{
    public NewTourneyViewModel()
    {
        AllTeams = TourneyManager.Instance.Teams;
        
        TourneyManager.Instance.Teams.CollectionChanged += (sender, args) => UpdateTeams();
        SelectedTeams.CollectionChanged += (sender, args) => SelectionChanged();
    }

    public event EventHandler TourneyStarted;
    
    public ObservableCollection<Team> AllTeams { get; set; }

    [ObservableProperty] 
    private int _selectedTeamsCount = 0;
    public ObservableCollection<Team> SelectedTeams { get; set; } = new ObservableCollection<Team>();
    
    [ObservableProperty]
    private bool _canStartTourney = false;
    
    [ObservableProperty]
    private string _tourneyName = "";
    
    public void SelectionChanged()
    {
        SelectedTeamsCount = SelectedTeams.Count;
        CanStartTourney = SelectedTeamsCount == 8;
    }
    
    [RelayCommand]
    private void StartTourney()
    {
        TourneyManager.Instance.StartTourney(TourneyName, SelectedTeams);
        
        OnTourneyStarted();
    }
    protected virtual void OnTourneyStarted()
    {
        TourneyStarted?.Invoke(this, EventArgs.Empty);
    }
    
    public void UpdateTeams()
    {
        AllTeams = TourneyManager.Instance.Teams;
    }
}