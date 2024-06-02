using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Tourney.Views;

public partial class NewTourneyView : UserControl
{
    public NewTourneyView()
    {
        InitializeComponent();
    }
    
    private void NumericUpDown_OnPreviewTextInput(object sender, TextInputEventArgs e)
    {
        e.Handled = true;
    }
}