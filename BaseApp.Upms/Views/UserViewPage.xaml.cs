using BaseApp.Upms.ViewModels;
using Wpf.Ui.Controls;

namespace BaseApp.Upms.Views
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserViewPage : INavigableView<UserViewModel>
    {
        public UserViewModel ViewModel { get; }

        public UserViewPage(UserViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
