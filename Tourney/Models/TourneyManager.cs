using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tourney.Io;

namespace Tourney.Models;

// Singleton class that manages the entire application
public partial class TourneyManager : ObservableObject
{
    public TourneyManager()
    {
    }
    public static void Initialize()
    {
        if (_instance != null)
        {
            return;
        }
        
        TourneyManagerDto? dto = Serializer.Deserialize();
        if (dto is null)
        {
            _instance = new TourneyManager();
            _instance.Teams = new ObservableCollection<Team>
            {
                new ("DanoDrevo", "rad odpisujem vodomery 🛁", "#cc6666"),
                new ("Zapalkari", "🤙🤙", "#cc9c66"),
                new ("zebrak", "lorem ipsum", "#c2cc66"),
                new ("lgbt", "he/he 🤡", "#7acc66"),
                new ("Vagosi", "<3 suche lepidlo", "#66ccaf"),
                new ("ಠ_ಠ", "👀", "#666dcc"),
                new ("Dong", "homosex", "#a866cc"),
                new ("Brezno", "🤢🤮🤮🤮🤢", "#cc66b3"),
                new ("Peter1v9", "copium", "#cc6666"),
                new ("average", "to nebol moj flash", "#cc9c66"),
                new ("BIG TONKA T", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "#666dcc"),
            };
        }
        else
        {
            _instance = dto.ToTourneyManager();
        }
        
        _instance.PropertyChanged += (sender, args) => Serializer.Serialize();
        _instance.Teams.CollectionChanged += (sender, args) => Serializer.Serialize();
        _instance.Tourneys.CollectionChanged += (sender, args) => Serializer.Serialize();
    }
    
    private static TourneyManager? _instance;

    public static TourneyManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Initialize();
            }
            return _instance!;
        }
    }

    public ObservableCollection<Team> Teams { get; set; } = new();
    
    [ObservableProperty]
    private AbstractTourney? _currentTourney;
    
    public ObservableCollection<AbstractTourney> Tourneys { get; set; } = new();
    
    public event EventHandler CurrentTourneyChanged;
    
    public void StartEliminationTourney(string name, IEnumerable<Team> teams)
    {
        CurrentTourney = new EliminationTourney(name, teams);
        CurrentTourney.PropertyChanged += (sender, args) => CurrentTourneyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void StartVersusTourney(string name, Team team1, Team team2, int rounds)
    {
        CurrentTourney = new VersusTourney(name, rounds, team1, team2);
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
    
    public void ChangeDescription(Team team, string description)
    {
        team.Description = description;
        OnPropertyChanged();
    }
    
    public void ChangeColor(Team team, string color)
    {
        team.Color = color;
        OnPropertyChanged();
    }
    
    public void ChangeIcon(Team team, string icon)
    {
        team.IconPath = icon;
        OnPropertyChanged();
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