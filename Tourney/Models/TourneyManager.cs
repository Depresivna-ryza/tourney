using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tourney.Models;

// Singleton class that manages the entire application
public partial class TourneyManager : ObservableObject
{
    public TourneyManager()
    {
        Teams = new ObservableCollection<Team>
        {
            new Team("Team 1", "Description 1", "#FF0000"),
            new Team("Team 2", "Description 2", "#00FF00"),
            new Team("Team 3", "Description 3", "#0000FF"),
            new Team("Team 4", "Description 4", "#FFFF00"),
            new Team("Team 5", "Description 5", "#FF00FF"),
            new Team("Team 6", "Description 6", "#00FFFF"),
            new Team("Team 7", "Description 7", "#FFFFFF"),
            new Team("Team 8", "Description 8", "#000000")
        };
    }
    private static TourneyManager? _instance;

    public static TourneyManager Instance
    {
        get { return _instance ??= new TourneyManager(); }
    }
    
    public ObservableCollection<Team> Teams { get; }
    
    [ObservableProperty]
    private Tourney? _currentTourney;
    
    public ObservableCollection<Tourney> Tourneys { get; } = new();
    
    public event EventHandler CurrentTourneyChanged;
    
    public void StartTourney(string name, IEnumerable<Team> teams)
    {
        CurrentTourney = new Tourney(name, teams);
        CurrentTourney.PropertyChanged += (sender, args) => CurrentTourneyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void DiscardCurrentTourney()
    {
        CurrentTourney = null;
        CurrentTourneyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public bool SaveCurrentTourney()
    {
        if (CurrentTourney is null)
        {
            return false;
        }
        
        if (CurrentTourney.Round3.Count == 0 || CurrentTourney.Round3[0].Winner == null)
        {
            return false;
        }
        
        CurrentTourney.Winner = CurrentTourney.Round3[0].Winner;
        Tourneys.Add(CurrentTourney);
        CurrentTourney = null;
        CurrentTourneyChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }



    public bool CanAddTeam(string name)
    {
        return Teams.All(team => team.Name != name);
    }
    
    public bool AddTeam(string name, string description, string color)
    {
        if (!CanAddTeam(name))
        {
            return false;
        }
        
        Teams.Add(new Team(name, description, color));
        return true;
    }
    
    public bool RemoveTeam(string name)
    {
        var team = Teams.FirstOrDefault(team => team.Name == name);
        if (team is null)
        {
            return false;
        }
        
        Teams.Remove(team);
        return true;
    }
    
    public bool ChangeDescription(string name, string description)
    {
        var team = Teams.FirstOrDefault(team => team.Name == name);
        if (team is null)
        {
            return false;
        }
        team.Description = description;
        return true;
    }
}