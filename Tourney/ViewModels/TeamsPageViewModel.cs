using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        
        NewIconPath = IconPaths[new Random().Next(IconPaths.Count)];
        NewColor = PredefinedColors[new Random().Next(PredefinedColors.Count)];
    }
    
    public ObservableCollection<string> PredefinedColors { get; } =
        ["#cc6666", "#cc9c66", "#c2cc66", "#7acc66", "#66ccaf", "#666dcc", "#a866cc", "#cc66b3"];

    public ObservableCollection<string> IconPaths { get; } = new();
    
    [ObservableProperty] 
    private ObservableCollection<Team> _teams;
    
    [ObservableProperty]
    private string _teamFilter = "";
    
    [ObservableProperty] 
    private Team? _selectedTeam;
    
    [ObservableProperty] 
    private bool _isSelectedTeam;
    
    [ObservableProperty] 
    private string _newDescription  = "";
    
    [ObservableProperty]
    private bool _isEditDescription = false;

    [ObservableProperty] 
    private string _newIconPath;

    [ObservableProperty] 
    private string _newColor;
    
    [ObservableProperty]
    private bool _isEditIcon = false;
    
    [RelayCommand]
    private void EditDescription()
    {
        IsEditDescription = true;
        NewDescription = SelectedTeam.Description;
    }
    
    [RelayCommand]
    private void SaveDescription()
    {
        IsEditDescription = false;
        
        SelectedTeam.Description = NewDescription;
        NewDescription = "";
    }
    
    [RelayCommand]
    private void DiscardDescription()
    {
        IsEditDescription = false;
        NewDescription = "";
    }
    
    [RelayCommand]
    private void EditIcon()
    {
        IsEditIcon = true;
    }
    
    [RelayCommand]
    private void SaveIcon()
    {
        IsEditIcon = false;
        SelectedTeam.IconPath = NewIconPath;
        SelectedTeam.Color = NewColor;
    }
    
    [RelayCommand]
    private void DiscardIcon()
    {
        IsEditIcon = false;
    }
    
    public void UpdateTeams()
    {
        Teams = TourneyManager.Instance.Teams;
    }
    
    [RelayCommand]
    private void RemoveTeam()
    {
        if (SelectedTeam == null)
            return;
        
        TourneyManager.Instance.Teams.Remove(SelectedTeam);
        SelectedTeam = null;
    }

    partial void OnSelectedTeamChanged(Team? value)
    {
        IsSelectedTeam = value != null;
        
        IsEditDescription = false;
        IsEditIcon = false;
        
        NewDescription = "";
    }
    
    partial void OnTeamFilterChanged(string value)
    {
        Teams = new ObservableCollection<Team>(TourneyManager.Instance.Teams.Where(t => t.Name.ToLower().Contains(TeamFilter.ToLower())));
    }
}