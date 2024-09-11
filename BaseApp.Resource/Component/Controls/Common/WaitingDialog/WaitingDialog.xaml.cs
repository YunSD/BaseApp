using System.Windows.Controls;

namespace BaseApp.Resource.Controls;

public partial class WaitingDialog : UserControl
{

    public WaitingDialog(string text = "waiting")
    {
        DataContext = this;
        InitializeComponent();
        TextBlock.Text = text;
    }
}
