using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tourney.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] 
    private bool _isPaneOpen = false;
    
    [ObservableProperty]
    private string _title = "Tourney";

    [RelayCommand]
    private void TogglePaneOpen()
    {
        IsPaneOpen = !IsPaneOpen;
        ToggleTextLabels();
    }
    
    private void ToggleTextLabels()
    {
        if (IsPaneOpen)
        {
            foreach (var item in PageItems)
            {
                item.Label = item.HiddenLabel;
            }
        }
        else
        {
            foreach (var item in PageItems)
            {
                item.Label = "";
            }
        }
        // trigger event collection changed
        PageItems = new ObservableCollection<MainPageItem>(PageItems);
    }

    public ObservableCollection<MainPageItem> PageItems { get; set; } =
    [
        // new MainPageItem("Home", new HomePageViewModel(), "HomeIcon"),
        new MainPageItem("Teams", new TeamsPageViewModel(), "TeamIcon"),
        new MainPageItem("Tourneys", new TourneysPageViewModel(), "TourneyIcon"),
        new MainPageItem("Add Team", new AddTeamPageViewModel(), "AddTeamIcon"),
        new MainPageItem("New Tourney", new TourneyPageViewModel(), "AddIcon"),
    ];

    [ObservableProperty] 
    private MainPageItem _currentPageItem;

    
    public MainWindowViewModel()
    {
        CurrentPageItem = PageItems[0];
        TogglePaneOpen();   
    }
}

public partial class MainPageItem : ObservableObject
{
    [ObservableProperty] 
    private string _label;
    public string HiddenLabel { get; set; }
    public ViewModelBase ViewModel { get; set; }
    public StreamGeometry Icon { get; set; }
    
    public MainPageItem(string label, ViewModelBase viewModel, string iconKey)
    {
        Label = label;
        HiddenLabel = label;
        ViewModel = viewModel;
        Application.Current!.TryFindResource(iconKey, out var res);
        Icon = (StreamGeometry) res!;
    }
    
}