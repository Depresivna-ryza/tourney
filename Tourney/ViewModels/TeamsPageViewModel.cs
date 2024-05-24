using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class TeamsPageViewModel : ViewModelBase
{
    public TeamsPageViewModel()
    {
        Teams = TourneyManager.Instance.Teams;
        SelectedTeam = Teams.Count > 0 ? Teams[0] : null;
        
        // subscribe to model changes
        TourneyManager.Instance.Teams.CollectionChanged += (sender, args) => UpdateTeams();
    }
    public ObservableCollection<Team> Teams { get; set; }
    
    [ObservableProperty] 
    private Team? _selectedTeam;
    public void UpdateTeams()
    {
        Teams = TourneyManager.Instance.Teams;
    }

    [ObservableProperty] 
    private string _newTeamName = "enter team name here";
    
    [ObservableProperty] 
    private string _newTeamDescription  = "enter team description here";
    
    [RelayCommand]
    private void AddTeam()
    {
        if (string.IsNullOrWhiteSpace(NewTeamName))
            return;

        var team = new Team(NewTeamName, NewTeamDescription, "#1e1e1e");
        TourneyManager.Instance.Teams.Add(team);
        NewTeamName = "";
        NewTeamDescription = "";
    }
    
    [RelayCommand]
    private void RemoveTeam()
    {
        if (SelectedTeam == null)
            return;
        
        TourneyManager.Instance.Teams.Remove(SelectedTeam);
        SelectedTeam = Teams.Count > 0 ? Teams[0] : null;
    }
}