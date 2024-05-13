using System.Collections.ObjectModel;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Kernel;
using Tourney.Models;

namespace Tourney.ViewModels;

public partial class AddTeamPageViewModel : ViewModelBase
{
    private Timer _clearMessageTimer;
    public AddTeamPageViewModel()
    {
        _clearMessageTimer = new Timer(1000);
        _clearMessageTimer.Elapsed += (sender, args) => Message = "";
        _clearMessageTimer.AutoReset = false; 
    }

    [ObservableProperty]
    private string _newTeamName = "";
    [ObservableProperty]
    private string _newTeamDescription = "";
    [ObservableProperty]
    private string _message = "";

    [ObservableProperty] private string _color = "Green";
    
    [RelayCommand]
    private void AddTeam()
    {
        _clearMessageTimer.Start();
        if (string.IsNullOrWhiteSpace(NewTeamName))
        {
            Message = "Team name cannot be empty";
            Color = "Red";
            return;
        }
        if (!TourneyManager.Instance.CanAddTeam(NewTeamName))
        {
            Message = "Team name already exists";
            Color = "Red";
            return;
        }
        var team = new Team(NewTeamName, NewTeamDescription, "#1e1e1e");
        
        TourneyManager.Instance.Teams.Add(team);
        Message = $"Team {NewTeamName} added successfully";
        NewTeamName = "";
        NewTeamDescription = "";
        Color = "Green";
        
    }

}