using MaterialDemo.ViewModels.Pages.Upms;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Upms
{
    public partial class RoleMenuEditorView : UserControl
    {
        public RoleMenuSelectViewModel ViewModel { get; }

        public RoleMenuEditorView(RoleMenuSelectViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        private void TreeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine();
        }
    }
}
