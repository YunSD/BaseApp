using BaseApp.Upms.Views;
using BaseApp.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace BaseApp.Views
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeViewPage : Page, INavigationWindow
    {

        public HomeViewModel NavigationItems { get; set; }
        public MainWindowViewModel MainWindowViewModel { get; set; }
        public MainWindow MainWindowView { get; set; }

        public HomeViewPage(IPageService pageService, INavigationService navigationService, HomeViewModel homeViewModel, MainWindowViewModel MainWindowViewModel, MainWindow mainWindowView)
        {
            this.NavigationItems = homeViewModel;
            this.MainWindowViewModel = MainWindowViewModel;
            this.MainWindowView = mainWindowView;

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

        private void KeyboardVariant_Button_Click(object sender, RoutedEventArgs e)
        {
            KeyboardHelper.ShowOnScreenKeyboard();
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView.SetCurrentValue(Window.WindowStateProperty, WindowState.Minimized);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView.Close();
        }
    }
}
