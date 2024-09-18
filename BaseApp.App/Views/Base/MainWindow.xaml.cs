using BaseApp.App.Windows;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.Utils;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;

namespace BaseApp.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IRecipient<LoginCompletedRedirectionMessage>, IRecipient<LogoutMessage>
    {
        #region ViewModel
        public MainWindowViewModel ViewModel { get; }
        #endregion

        public MainWindow(MainWindowViewModel viewModel, LoginViewPage loginViewPage)
        {
            this.ViewModel = viewModel;

            this.DataContext = this;

            InitializeComponent();

            this.Navigate(loginViewPage);

            // default
            MainFrame.Navigated += (sender, e) => { while (MainFrame.NavigationService.RemoveBackEntry() != null) ; };

            // register message
            WeakReferenceMessenger.Default.Register<LoginCompletedRedirectionMessage>(this);
            WeakReferenceMessenger.Default.Register<LogoutMessage>(this);

            // 初始化 SnackbarPresenter
            SnackbarService.Singleton.SetSnackbarPresenter(SnackbarPresenter);
        }

        #region Window methods

        public void Navigate(Page page)
        {
            MainFrame.Navigate(page);
        }


        public void ShowWindow() => base.Show();

        public void CloseWindow() => base.Close();

        #endregion Window methods


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }


        public void Receive(LoginCompletedRedirectionMessage message)
        {
            this.Navigate(ServiceProviderUtil.GetRequiredService<HomeViewPage>());
        }

        public void Receive(LogoutMessage message)
        {
            this.Navigate(ServiceProviderUtil.GetRequiredService<LoginViewPage>());
        }
    }

}