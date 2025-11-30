using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tourney.Models;

namespace Tourney.Io;

public class TeamDto
{
    public TeamDto()
    {
    }
    
    public TeamDto(Team team)
    {
        Name = team.Name;
        Icon = team.IconPath;
        Color = team.Color;
        Description = team.Description;
    }
    
    public string Name { get; set; }
    public string Icon { get; set; } 
    public string Color { get; set; }
    public string Description { get; set; }
    
    public Team ToTeam()
    {
        return new(Name, Description, Color, Icon);
    }
}

public class EliminationMatchDto
{
    public EliminationMatchDto()
    {
    }
    public EliminationMatchDto(EliminationMatch match)
    {
        Team1 = match.Team1 == null ? null : new TeamDto(match.Team1);
        Team2 = match.Team2 == null ? null : new TeamDto(match.Team2);
        Winner1 = match.Winner == match.Team1;
        Duration = match.Duration;
        IsFinal = match.IsFinal;
        NextMatchSlot1 = match.NextMatchSlot1;
        State = match.State;
    }
    public TeamDto? Team1 { get; set; }
    public TeamDto? Team2 { get; set; }
    public bool Winner1 { get; set; }

    public TimeSpan Duration { get; set; }
    public bool IsFinal { get; set; }
    public bool NextMatchSlot1 { get; set; }
    public EliminationMatch.MatchState State { get; set;}
    
    public EliminationMatch ToEliminationMatch()
    {
        var match = new EliminationMatch();
        match.Team1 = Team1?.ToTeam();
        match.Team2 = Team2?.ToTeam();
        match.Winner = Winner1 ? match.Team1 : match.Team2;
        match.Duration = Duration;
        match.IsFinal = IsFinal;
        match.NextMatchSlot1 = NextMatchSlot1;
        match.State = State;
        return match;
    }
}


public class EliminationTourneyDto
{
    public EliminationTourneyDto()
    {
    }
    public EliminationTourneyDto(EliminationTourney tourney)
    {
        Name = tourney.Name;
        StartDate = tourney.StartDate;
        Winner = tourney.Winner == null ? null : new TeamDto(tourney.Winner);
        Rounds = new();
        foreach (var round in tourney.Rounds)
        {
            var roundDto = new List<EliminationMatchDto>();
            foreach (var match in round)
            {
                roundDto.Add(new(match));
            }
            Rounds.Add(roundDto);
        }
    }
    
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public TeamDto? Winner { get; set; }
    public List<List<EliminationMatchDto>> Rounds { get; set; }
    
    public EliminationTourney ToEliminationTourney()
    {
        var tourney = new EliminationTourney();
        tourney.Name = Name;
        tourney.StartDate = StartDate;
        tourney.Winner = Winner?.ToTeam();
        foreach (var round in Rounds)
        {
            var roundList = new ObservableCollection<EliminationMatch>();
            foreach (var match in round)
            {
                roundList.Add(match.ToEliminationMatch());
            }
            tourney.Rounds.Add(roundList);
        }
        return tourney;
    }
}

public class VersusTourneyDto
{
    public VersusTourneyDto()
    {
    }
    public VersusTourneyDto(VersusTourney tourney)
    {
        Name = tourney.Name;
        StartDate = tourney.StartDate;
        Team1 = new(tourney.Team1);
        Team2 = new(tourney.Team2);
        Winner = tourney.Winner == null ? null : new TeamDto(tourney.Winner);
        TotalRounds = tourney.TotalRounds;
        CurrentRound = tourney.CurrentRound;
        
        Rounds = new();
        foreach (var round in tourney.Rounds)
        {
            Rounds.Add(round);
        }
    }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public TeamDto Team1 { get; set; }
    public TeamDto Team2 { get; set; }
    public TeamDto? Winner { get; set; }
    public List<bool> Rounds { get; set; }
    
    public int TotalRounds { get; set; }
    public int CurrentRound { get; set; }
    
    public VersusTourney ToVersusTourney()
    {
        var tourney = new VersusTourney(Name, TotalRounds, Team1.ToTeam(), Team2.ToTeam());
        tourney.StartDate = StartDate;
        tourney.Winner = Winner?.ToTeam();
        tourney.CurrentRound = CurrentRound;
        foreach (var round in Rounds)
        {
            tourney.Rounds.Add(round);
        }
        return tourney;
    }
}


public class TourneyManagerDto
{
    public TourneyManagerDto()
    {
    }

    public TourneyManagerDto(TourneyManager manager)
    {
        EliminationTourneys = new();
        VersusTourneys = new();
        foreach (var tourney in manager.Tourneys)
        {
            if (tourney is VersusTourney versusTourney)
            {
                VersusTourneys.Add(new(versusTourney));
            }
            else if (tourney is EliminationTourney eliminationTourney)
            {
                EliminationTourneys.Add(new(eliminationTourney));
            }
        }
        
        Teams = new();
        foreach (var team in manager.Teams)
        {
            Teams.Add(new(team));
        }
    }
    
    public List<EliminationTourneyDto> EliminationTourneys { get; set; }
    public List<VersusTourneyDto> VersusTourneys { get; set; }
    
    public List<TeamDto> Teams { get; set; }
    
    public TourneyManager ToTourneyManager()
    {
        TourneyManager manager = new();
        foreach (var team in Teams)
        {
            manager.Teams.Add(team.ToTeam());
        }
        
        foreach (var tourney in EliminationTourneys)
        {
            manager.Tourneys.Add(tourney.ToEliminationTourney());
        }
        
        foreach (var tourney in VersusTourneys)
        {
            manager.Tourneys.Add(tourney.ToVersusTourney());
        }
        // sort tourneys by start date
        manager.Tourneys = new(manager.Tourneys.OrderBy(t => t.StartDate));

        return manager;
    }
}
