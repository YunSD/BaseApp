using MaterialDemo.ViewModels.Pages.Upms;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Upms
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
