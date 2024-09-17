using BaseApp.App.ViewModels;
using BaseApp.App.Windows;
using BaseApp.Core.Enums;
using BaseApp.Core.Security.Messages;
using BaseApp.Core.Utils;
using BaseApp.Resource.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using log4net;
using MaterialDesignThemes.Wpf;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Wpf.Ui;

namespace BaseApp.App.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginViewPage : Page, IRecipient<SwitchLoginMessage>, IRecipient<LoginCompletedRedirectionMessage>
    {

        #region ViewModel
        public LoginViewModel LoginViewModel { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        #endregion

        private ILog logger = LogManager.GetLogger(nameof(LoginViewPage));

        private OpenCvSharp.VideoCapture videoCapture;

        public LoginViewPage(LoginViewModel loginViewModel, MainWindowViewModel MainWindowViewModel)
        {
            this.MainWindowViewModel = MainWindowViewModel;
            this.LoginViewModel = loginViewModel;
            this.DataContext = this;
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<SwitchLoginMessage>(this);
            WeakReferenceMessenger.Default.Register<LoginCompletedRedirectionMessage>(this);

            
            InitializeCameraAsync();
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
            if (!videoCapture.IsOpened()) {
                SnackbarService.ShowError("摄像头无法正常打开。");
                return;
            }
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void UnloadCameraReader() {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private int frameCounter = 0;
        private OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (frameCounter++ % 5 != 0) return;
            if (videoCapture.IsOpened()) {
                videoCapture.Read(frame);
                Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 4, frame.Height / 4));
                if (!frame.Empty()) {
                    var bitmapSource = frame.ToBitmapSource();
                    FaceImage.Source = bitmapSource;
                }
            }
        }

        private async void InitializeCameraAsync() {
            await Task.Run(() =>
            {
                this.videoCapture = new OpenCvSharp.VideoCapture(0);
                this.videoCapture.Open(0);  // 通过摄像头索引打开摄像头
                
            });
            this.CompositionTarget_Rendering(this, null);
        }

        

        public async void Receive(LoginCompletedRedirectionMessage message)
        {
            await Task.Run(StopCamera);
        }

        private void StopCamera()
        {
            // 停止触发 CompositionTarget.Rendering 事件，停止帧捕获
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

            // 释放摄像头资源
            if (videoCapture != null && videoCapture.IsOpened())
            {
                videoCapture.Release(); // 释放摄像头资源
                videoCapture.Dispose();
            }

            if (frame != null)
            {
                frame.Dispose(); // 释放帧
            }
        }
    }
}

