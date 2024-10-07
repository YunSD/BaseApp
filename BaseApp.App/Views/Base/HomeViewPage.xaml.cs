using BaseApp.Upms.Views;
using BaseApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.Views
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeViewPage : Page, INavigationWindow
    {

        public HomeViewModel NavigationItems { get; set; }
        public MainWindowViewModel MainWindowViewModel { get; set; }

        public HomeViewPage(IPageService pageService, INavigationService navigationService, HomeViewModel homeViewModel, MainWindowViewModel MainWindowViewModel)
        {
            this.NavigationItems = homeViewModel;
            this.MainWindowViewModel = MainWindowViewModel;
            DataContext = this;
            InitializeComponent();
            this.Loaded += OnHomePageLoaded;

            SetPageService(pageService);
            navigationService.SetNavigationControl(RootNavigation);
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => throw new NotImplementedException();

        public void CloseWindow() => throw new NotImplementedException();

        public void SetServiceProvider(IServiceProvider serviceProvider) => RootNavigation.SetServiceProvider(serviceProvider);

        #endregion INavigationWindow methods

        private void NavigationToggle(object? sender, RoutedEventArgs? e)
        {
            RootNavigation.IsPaneOpen = !RootNavigation.IsPaneOpen;
        }


        private void OnHomePageLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not HomeViewPage)
            {
                return;
            }

            _ = Navigate(typeof(FolderViewPage));
        }

    }
}
