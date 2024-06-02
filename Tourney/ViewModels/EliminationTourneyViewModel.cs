using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Tourney.Models;

namespace Tourney.ViewModels;
// MatchView, IsFirst, IsLast, DisplayEven, DisplayOdd
using MatchTuple = Tuple<EliminationMatchViewModel, bool , bool, bool, bool>;
public partial class EliminationTourneyViewModel : ViewModelBase
{

    public EliminationTourneyViewModel(EliminationTourney? tourney)
    {
        UpdateTourney(tourney);
        if (tourney != null)
        {
            tourney.PropertyChanged += (sender, args) => UpdateTourney(tourney);
        }
    }
    
    public ObservableCollection<ObservableCollection<MatchTuple>> Rounds { get; set; } = new();
    
    [ObservableProperty] 
    private EliminationTourney? _tourney;
    
    [ObservableProperty]
    private string _trophyColor = "#202020";
    
    [ObservableProperty]
    private string _winnerName = "";
    
    public void UpdateTourney(EliminationTourney? newTourney)
    {
        
        if (newTourney == null)
        {
            return;
        }
        
        if ( newTourney == Tourney)
        {
            TrophyColor = newTourney.Winner != null ? "White" : "#202020";
            WinnerName = newTourney.Winner?.Name ?? "";
            return;
        }
        
        Rounds.Clear();

        Tourney = newTourney;
        
        for (int i = 0; i < Tourney.Rounds.Count; i++)
        {
            var round = Tourney.Rounds[i];
            var roundViewModel = new ObservableCollection<MatchTuple>();
            for (int j = 0; j < round.Count; j++)
            {
                var match = round[j];
                
                bool isFirst = i == 0;
                bool isLast = i == Tourney.Rounds.Count - 1;
                bool displayEven = j % 2 == 0 && !isLast;
                bool displayOdd = j % 2 == 1 && !isLast;
                roundViewModel.Add(new MatchTuple(new EliminationMatchViewModel(match), isFirst, isLast, displayEven, displayOdd));
            }
            Rounds.Add(roundViewModel);
        }
        
        TrophyColor = Tourney.Winner != null ? "White" : "#202020";
        WinnerName = Tourney.Winner?.Name ?? "";
    }

    
}