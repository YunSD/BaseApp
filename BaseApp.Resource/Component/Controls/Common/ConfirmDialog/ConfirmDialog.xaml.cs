using System.Windows.Controls;

namespace BaseApp.Resource.Controls;

/// <summary>
/// Interaction logic for SampleDialog.xaml
/// </summary>
public partial class ConfirmDialog : UserControl
{

    public ConfirmDialog(string message)
    {
        DataContext = this;
        InitializeComponent();
        Title.Text = message;
    }
}
