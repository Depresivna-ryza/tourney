using System;
using System.IO;
using System.Text.Json;
using Tourney.Models;
namespace Tourney.Io;

public static class Serializer
{
    private static string _path = "tourney.json";

    private static string GetPath()
    {
        var directory = Directory.GetCurrentDirectory();
        while (!Directory.Exists(Path.Combine(directory, "Tourney")))
        {
            directory = Directory.GetParent(directory).FullName;
        }
        
        directory = Path.Combine(directory, "Tourney");

        if (Directory.Exists(Path.Combine(directory, "IO")))
        {
            directory = Path.Combine(directory, "IO");
        }
        
        return Path.Combine(directory, _path);
    }
    
    public static void Serialize()
    {
        TourneyManager manager = TourneyManager.Instance;
        TourneyManagerDto dto = new(manager);
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(dto, options);
        
        File.WriteAllText(GetPath(), json);
    }
    
    public static TourneyManagerDto? Deserialize()
    {
        try
        {
            var json = File.ReadAllText(GetPath());
            var dto = JsonSerializer.Deserialize<TourneyManagerDto>(json);
            return dto;
        }
        catch (FileNotFoundException e)
        {
        }

        return null;
    }
}