namespace Tourney.Models;

public class Team
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string IconPath { get; set; }
    
    public Team(string name, string description, string color, string icon="/Assets/Icons/turd-svgrepo-com.svg")
    {
        Name = name;
        Description = description;
        Color = color;
        IconPath = icon;
    }
}