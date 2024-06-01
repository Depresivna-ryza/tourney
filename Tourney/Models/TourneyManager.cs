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
            new ("DanoDrevo", "rad odpisujem vodomery 🛁", "#FFFFFF"),
            new ("Zapalkari", "🤙🤙", "#FF0000"),
            new ("zebrak", "lorem ipsum", "#00FF00"),
            new ("lgbt", "he/he 🤡", "#0000FF"),
            new ("Vagosi", "<3 suche lepidlo", "#FFFF00"),
            new ("ಠ_ಠ", "👀", "#FF00FF"),
            new ("Dong", "homosex ", "#00FFFF"),
            new ("Brezno", "🤢🤮🤮🤮🤢", "#000000"),
            new ("Peter1v9", "copium", "#FFFFFF"),
        };
    }
    private static TourneyManager? _instance;

    public static TourneyManager Instance
    {
        get { return _instance ??= new TourneyManager(); }
    }
    
    public ObservableCollection<Team> Teams { get; }
    
    [ObservableProperty]
    private AbstractTourney? _currentTourney;
    
    public ObservableCollection<AbstractTourney> Tourneys { get; } = new();
    
    public event EventHandler CurrentTourneyChanged;
    
    public void StartEliminationTourney(string name, IEnumerable<Team> teams)
    {
        CurrentTourney = new EliminationTourney(name, teams);
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
        
        if (CurrentTourney.Winner == null)
        {
            return false;
        }
        
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