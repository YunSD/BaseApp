using BaseApp.Upms.ViewModels;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.Upms.Views
{
    public partial class FolderViewPage : INavigableView<FolderViewModel>
    {
        public FolderViewModel ViewModel { get; }
        public new INavigationService NavigationService { get; }

        public FolderViewPage(FolderViewModel viewModel, INavigationService navigationService)
        {
            this.ViewModel = viewModel;
            this.NavigationService = navigationService;
            DataContext = this;
            this.Loaded += OnHomePageLoaded;
            InitializeComponent();
        }


        private void OnHomePageLoaded(object sender, RoutedEventArgs e)
        {
            INavigationViewItem? navigationView = NavigationService.GetNavigationControl().SelectedItem;
            if (navigationView != null)
            {
                ViewModel.LoadNavigationItems(navigationView);
            }
        }
    }
}
