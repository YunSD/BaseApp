using BaseApp.App.ViewModels;
using BaseApp.App.Windows;
using BaseApp.Core.Utils;
using BaseApp.Resource.Controls;
using log4net;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui;

namespace BaseApp.App.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginViewPage : Page
    {

        #region ViewModel
        public LoginViewModel LoginViewModel { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        #endregion

        private ILog logger = LogManager.GetLogger(nameof(LoginViewPage));

        public LoginViewPage(LoginViewModel loginViewModel, MainWindowViewModel MainWindowViewModel)
        {
            this.MainWindowViewModel = MainWindowViewModel;
            this.LoginViewModel = loginViewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new WaitingDialog(), BaseConstant.BaseDialog);

            string password = PasswordBox.Password;

            bool flag = await LoginViewModel.Login(password);
            if (!flag) SnackbarService.ShowError("密码错误");

            if (DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) DialogHost.Close(BaseConstant.BaseDialog);
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Enter) SignIn_Click(sender, e);
        }
    }
}
