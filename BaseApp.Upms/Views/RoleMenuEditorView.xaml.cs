using BaseApp.Upms.ViewModels;
using System.Windows.Controls;

namespace BaseApp.Upms.Views
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
