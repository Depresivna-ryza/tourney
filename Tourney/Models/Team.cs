namespace Tourney.Models;

public class Team
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    
    public Team(string name, string description, string color)
    {
        Name = name;
        Description = description;
        Color = color;
    }
}