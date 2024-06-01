using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;
using Avalonia.Collections;

namespace Tourney.ViewModels;

public partial class NewTourneyViewModel : ViewModelBase
{
    public NewTourneyViewModel()
    {
        UpdateTeams();
         
        TourneyManager.Instance.Teams.CollectionChanged += (sender, args) => UpdateTeams();
        SelectedTeams.CollectionChanged += (sender, args) => SelectedTeamsChanged();
    }

    public event EventHandler TourneyStarted;

    [ObservableProperty] 
    private ObservableCollection<Tuple<Team, bool>> _allTeams;
    
    [ObservableProperty] 
    private ObservableCollection<Team> _filteredTeams;
    
    [ObservableProperty] 
    private int _selectedTeamsCount = 0;
    
    [ObservableProperty] 
    private ObservableCollection<Team> _selectedTeams = new();    
    
    [ObservableProperty] 
    private ObservableCollection<Team> _selectedTeamsFiltered = new();
    
    [ObservableProperty]
    private bool _canStartTourney = false;
    
    [ObservableProperty]
    private string _tourneyName = "";
    
    [ObservableProperty]
    private string _teamFilter = "";
    
    public void SelectedTeamsChanged()
    {
        var newAllTeams = new ObservableCollection<Tuple<Team, bool>>();
        foreach (var x in AllTeams)
        {
            if (FilteredTeams.Contains(x.Item1))
            {
                bool contains = SelectedTeams.Contains(x.Item1);
                
                newAllTeams.Add(new Tuple<Team, bool>(x.Item1, contains));
            }
            else
            {
                newAllTeams.Add(x);
            }
        }
        
        AllTeams = newAllTeams;
        // UpdateFilteredTeams();
        //
        SelectedTeamsFiltered = new ObservableCollection<Team>(AllTeams.Where(t => t.Item2).Select(t => t.Item1));
        SelectedTeamsCount = SelectedTeamsFiltered.Count;
        
        UpdateCanStartTourney();
    }

    partial void OnTourneyNameChanged(string value)
    {
        UpdateCanStartTourney();
    }

    partial void OnTeamFilterChanged(string value)
    {
        UpdateFilteredTeams();
        
        var newSelectedTeams = AllTeams
            .Where(t => t.Item1.Name.ToLower().Contains(TeamFilter.ToLower()))
            .Where(t => t.Item2)
            .Select(t => t.Item1)
            .ToList();

        SelectedTeams.Clear();
        foreach (var team in newSelectedTeams)
        {
            SelectedTeams.Add(team);
        }
    }

    public void UpdateCanStartTourney()
    {
        CanStartTourney = SelectedTeamsCount >= 2 && 
                          !string.IsNullOrWhiteSpace(TourneyName) && 
                          TourneyName.Length > 3;
    }
    
    [RelayCommand]
    private void StartTourney()
    {
        TourneyManager.Instance.StartEliminationTourney(TourneyName, SelectedTeamsFiltered);
        TourneyName = "";
        SelectedTeams.Clear();
        
        OnTourneyStarted();
    }
    protected virtual void OnTourneyStarted()
    {
        TourneyStarted?.Invoke(this, EventArgs.Empty);
    }
    
    public void UpdateTeams()
    {
        var teams  = TourneyManager.Instance.Teams;
        AllTeams = new ObservableCollection<Tuple<Team, bool>>(teams.Select(t => new Tuple<Team, bool>(t, false)));

        UpdateFilteredTeams();
    }
    
    public void UpdateFilteredTeams()
    {
        FilteredTeams = new ObservableCollection<Team>(AllTeams.Where(t => t.Item1.Name.ToLower().Contains(TeamFilter.ToLower())).Select(t => t.Item1));
    }
}