using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tourney.Models;

public abstract partial class AbstractTourney : ObservableObject
{
    [ObservableProperty] 
    private string _name;
    
    [ObservableProperty]
    private Team? _winner;
    
    [ObservableProperty]
    private DateTime _startDate = DateTime.Now;
}