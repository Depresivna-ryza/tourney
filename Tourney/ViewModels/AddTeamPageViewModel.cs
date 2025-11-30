using System;
using System.Collections.ObjectModel;
using System.IO;
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
        
        // find root directory
        string dir = Environment.CurrentDirectory;
        while (!Directory.Exists(Path.Combine(dir, "Assets")))
        {
            dir = Directory.GetParent(dir).FullName;
        }
        string iconDir = Path.Combine(dir, "Assets", "Icons");
        foreach (string filePath in Directory.GetFiles(iconDir, "*.svg"))
        {
            IconPaths.Add(filePath);
        }
        
        SelectedIconPath = IconPaths[new Random().Next(IconPaths.Count)];
        SelectedColor = PredefinedColors[new Random().Next(PredefinedColors.Count)];
    }

    [ObservableProperty]
    private string _newTeamName = "";
    [ObservableProperty]
    private string _newTeamDescription = "";
    [ObservableProperty]
    private string _message = "";

    [ObservableProperty] private string _color = "Green";
    
    public ObservableCollection<string> Members { get; } = ["jano", "fero", "fero", "fero", "fero", "fero", "fero", "fero", "fero"];
    
    public ObservableCollection<string> IconPaths { get; } = [];

    [ObservableProperty] 
    private string _selectedIconPath;
    
    public ObservableCollection<string> PredefinedColors { get; } =
        ["#cc6666", "#cc9c66", "#c2cc66", "#7acc66", "#66ccaf", "#666dcc", "#a866cc", "#cc66b3"];
    
    [ObservableProperty]
    private string _selectedColor;

    [RelayCommand]
    private void AddMember()
    {
        Members.Add($"Member {Members.Count + 1}");
    }
    
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
        var team = new Team(NewTeamName, NewTeamDescription, SelectedColor, SelectedIconPath);
        
        TourneyManager.Instance.Teams.Add(team);
        Message = $"Team {NewTeamName} added successfully";
        NewTeamName = "";
        NewTeamDescription = "";
        Color = "Green";
        
    }

}