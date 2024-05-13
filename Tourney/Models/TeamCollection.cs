// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using CommunityToolkit.Mvvm.ComponentModel;
//
// namespace Tourney.Models;
//
// public class TeamCollection : ObservableObject
// {
//     public ObservableCollection<Team> Teams { get; }
//     
//     public TeamCollection()
//     {
//         // mock data
//         Teams = new ObservableCollection<Team>
//         {
//             new Team("Team 1", "Description 1", "#FF0000"),
//             new Team("Team 2", "Description 2", "#00FF00"),
//             new Team("Team 3", "Description 3", "#0000FF"),
//             new Team("Team 4", "Description 4", "#FFFF00"),
//             new Team("Team 5", "Description 5", "#FF00FF"),
//             new Team("Team 6", "Description 6", "#00FFFF"),
//             new Team("Team 7", "Description 7", "#FFFFFF"),
//             new Team("Team 8", "Description 8", "#000000")
//         };
//     }
//     
//     public bool CanAddTeam(string name)
//     {
//         return Teams.All(team => team.Name != name);
//     }
//     
//     public bool AddTeam(string name, string description, string color)
//     {
//         if (!CanAddTeam(name))
//         {
//             return false;
//         }
//         
//         Teams.Add(new Team(name, description, color));
//         return true;
//     }
//     
//     public bool RemoveTeam(string name)
//     {
//         var team = Teams.FirstOrDefault(team => team.Name == name);
//         if (team is null)
//         {
//             return false;
//         }
//         
//         Teams.Remove(team);
//         return true;
//     }
//     
//     public bool ChangeDescription(string name, string description)
//     {
//         var team = Teams.FirstOrDefault(team => team.Name == name);
//         if (team is null)
//         {
//             return false;
//         }
//         team.Description = description;
//         return true;
//     }
//     
//     public bool ChangeColor(string name, string color)
//     {
//         var team = Teams.FirstOrDefault(team => team.Name == name);
//         if (team is null)
//         {
//             return false;
//         }
//         team.Color = color;
//         return true;
//     }
// }
