using MaterialDemo.ViewModels.Pages.Upms;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Upms
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
