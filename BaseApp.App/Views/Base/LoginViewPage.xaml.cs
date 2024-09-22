using BaseApp.App.Services;
using BaseApp.App.Utils;
using BaseApp.App.ViewModels;
using BaseApp.App.Windows;
using BaseApp.Core.Enums;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.Utils;
using BaseApp.Resource.Controls;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using MaterialDesignThemes.Wpf;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui;

namespace BaseApp.App.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginViewPage : Page, IRecipient<SwitchLoginMessage>, IRecipient<LoginCompletedMessage>
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
            WeakReferenceMessenger.Default.Register<SwitchLoginMessage>(this);
            WeakReferenceMessenger.Default.Register<LoginCompletedMessage>(this);

        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new WaitingDialog(), BaseConstant.BaseDialog);

            string password = PasswordBox.Password;

            bool flag = await LoginViewModel.LoginByPassword(password);
            if (!flag) SnackbarService.ShowError("密码错误");

            if (DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) DialogHost.Close(BaseConstant.BaseDialog);
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Enter) SignIn_Click(sender, e);
        }

        public void Receive(SwitchLoginMessage message)
        {
            // 停止摄像头采样
            UnloadCameraReader();
            switch (message.LoginType)
            {
                case LoginType.CARD:
                    LoadICReader();
                    break;
                case LoginType.FINGERPRINT:
                    LoadFingerPrintReader();
                    break;
                case LoginType.FACE_RECONGNITION:
                    LoadCameraReader();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// IC 读卡器
        /// </summary>
        private void LoadICReader()
        {

        }

        /// <summary>
        /// 指纹读卡器
        /// </summary>
        private void LoadFingerPrintReader()
        {

        }



        /// <summary>
        /// 摄像头
        /// </summary>
        private void LoadCameraReader()
        {
            if (!CameraService.IsOpened())
            {
                SnackbarService.ShowError("摄像头未正常打开,正在重试请稍后。");
                Task.Run(() => CameraService.ConnectCamera());
                return;
            }
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }


        private SemaphoreSlim IsProcess = new SemaphoreSlim(1, 1);
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (!IsProcess.Wait(0)) return;
            Task.Run(async () => {
            try
            {
                await Task.Delay(50);
                using (OpenCvSharp.Mat frame = CameraService.Read())
                {
                    if (!frame.Empty())
                    {
                        if (CVUtil.FaceDetect(frame))
                        {
                            CVUtil.AntiSpoofingDemo(frame);
                            LoginViewModel.LoginByFaceRecognition(frame);
                        }
                        Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 4, frame.Height / 4));

                        Dispatcher.Invoke(() =>
                        {
                            BitmapSource bitmapSource = frame.ToBitmapSource();
                            FaceImage.Source = bitmapSource;
                        });
                        }
                    }
                }
                finally
                {
                    IsProcess.Release();
                }
            });
            
        }

        private void UnloadCameraReader()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        public void Receive(LoginCompletedMessage message)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            Dispatcher.Invoke(() => UnloadCameraReader());
        }
    }
}

