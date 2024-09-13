using BaseApp.Upms.ViewModels;
using System.Windows.Controls;

namespace BaseApp.Upms.Views
{
    public partial class RoleEditorView : UserControl
    {
        public RoleEditorViewModel ViewModel { get; }

        public RoleEditorView(RoleEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
