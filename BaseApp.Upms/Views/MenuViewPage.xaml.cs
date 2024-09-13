using BaseApp.Upms.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Upms.Views
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class MenuViewPage : INavigableView<MenuViewModel>
    {
        public MenuViewModel ViewModel { get; }

        public MenuViewPage(MenuViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void TreeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine();
        }
    }
}
