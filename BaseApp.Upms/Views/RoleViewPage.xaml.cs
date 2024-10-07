using BaseApp.Upms.ViewModels;
using Wpf.Ui.Controls;

namespace BaseApp.Upms.Views
{
    public partial class RoleViewPage : INavigableView<RoleViewModel>
    {
        public RoleViewModel ViewModel { get; }

        public RoleViewPage(RoleViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
