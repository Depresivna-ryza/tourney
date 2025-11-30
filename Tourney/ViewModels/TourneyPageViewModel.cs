using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.ViewModels;

public partial class TourneyPageViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ViewModelBase _currentViewModel;
    
    [ObservableProperty] 
    private NewTourneyViewModel _newTourneyViewModel;
    
    [ObservableProperty] 
    private OngoingTourneyViewModel _ongoingTourneyViewModel;
    
    
    public TourneyPageViewModel()
    {
        NewTourneyViewModel = new NewTourneyViewModel();
        OngoingTourneyViewModel = new OngoingTourneyViewModel();
        
        _newTourneyViewModel.TourneyStarted += (sender, args) =>
        {
            OngoingTourneyViewModel.StartTourney();
            CurrentViewModel = OngoingTourneyViewModel;
        };
        
        _ongoingTourneyViewModel.TourneyEnded += (sender, args) =>
        {
            CurrentViewModel = NewTourneyViewModel;
        };
        CurrentViewModel = NewTourneyViewModel;
    }
}