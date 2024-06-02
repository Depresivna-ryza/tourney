using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;

public partial class Team : ObservableObject
{
    public Team(string name, string description, string color, string icon="/Assets/Icons/turd-svgrepo-com.svg")
    {
        Name = name;
        Description = description;
        Color = color;
        IconPath = icon;
    }
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _description;
    [ObservableProperty] private string _color;
    [ObservableProperty] private string _iconPath;
    
}