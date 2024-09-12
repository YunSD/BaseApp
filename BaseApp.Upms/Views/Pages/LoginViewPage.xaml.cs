using log4net;
using MaterialDemo.Controls;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Base;
using MaterialDemo.ViewModels.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui;

namespace BaseApp.Upms.Pages.Base
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginViewPage : Page
    {

        #region ViewModel
        public MainWindowViewModel MainViewModel { get; }
        public LoginViewModel LoginViewModel { get; }
        #endregion

        private ILog logger = LogManager.GetLogger(nameof(LoginViewPage));

        public LoginViewPage(MainWindowViewModel mainViewModel, LoginViewModel loginViewModel)
        {
            this.MainViewModel = mainViewModel;
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
