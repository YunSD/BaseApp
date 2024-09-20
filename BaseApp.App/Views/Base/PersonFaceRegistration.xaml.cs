namespace BaseApp.App.Views
{
    using BaseApp.App.Services;
    using BaseApp.App.Utils;
    using BaseApp.App.ViewModels.Base;
    using BaseApp.Core.Utils;
    using BaseApp.Resource.Controls;
    using log4net;
    using MaterialDesignThemes.Wpf;
    using OpenCvSharp;
    using OpenCvSharp.WpfExtensions;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Wpf.Ui;

    /// <summary>
    /// Defines the <see cref="PersonFaceRegistration" />
    /// </summary>
    public partial class PersonFaceRegistration : UserControl
    {

        private ILog logger = LogManager.GetLogger(nameof(PersonFaceRegistration));
        private readonly PersonFaceRegistrationViewModel ViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonFaceRegistration"/> class.
        /// </summary>
        public PersonFaceRegistration(PersonFaceRegistrationViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
            DataContext = this;
            InitializeComponent();
            LoadCameraReader();
        }

        /// <summary>
        /// 摄像头
        /// </summary>
        private void LoadCameraReader()
        {
            if (!CameraService.IsOpened())
            {
                SnackbarService.ShowError("摄像头未正常打开。");
                return;
            }
            this.UnloadCameraReader();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private const int countdownSeconds = 5;

        private volatile bool isReady = false;

        private DateTime startTime = DateTime.Now;

        private SemaphoreSlim processSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// The CompositionTarget_Rendering
        /// </summary>
        /// <param name="sender">The sender<see cref="object?"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (!processSemaphore.Wait(0)) return;

            Task.Run(async () =>
            {
                bool isVerify = false;
                try
                {
                    await Task.Delay(50);

                    using (OpenCvSharp.Mat frame = CameraService.Read())
                    {
                        if (!frame.Empty())
                        {
                            Rect rect = CVUtil.ExpandRect(new Rect(0, 0, frame.Width, frame.Height), -60);

                            Scalar color = Scalar.Red;
                            if (CVUtil.FaceDetect(frame))
                            {
                                color = Scalar.White;
                                if (!isReady)
                                {
                                    isReady = true;
                                    startTime = DateTime.Now;
                                }

                                TimeSpan elapsed = DateTime.Now - startTime;
                                int remainingSeconds = countdownSeconds - (int)elapsed.TotalSeconds;
                                if (remainingSeconds >= 0)
                                {
                                    Point circleCenter = new Point(frame.Width / 2, 100);
                                    Cv2.Circle(frame, circleCenter, 50, Scalar.Red, 8);
                                    Cv2.PutText(frame, remainingSeconds.ToString(), new Point(circleCenter.X - 20, circleCenter.Y + 20), HersheyFonts.HersheySimplex, 2, Scalar.Red, 5);

                                }
                                else
                                {
                                    // 人像分析
                                    Dispatcher.Invoke(() =>
                                    {
                                        UnloadCameraReader();
                                        DialogHost.Show(new WaitingDialog("正在处理中..."), BaseConstant.RootDialog);
                                    });
                                    isVerify = true;
                                }
                            }
                            else
                            {
                                isReady = false;
                            }
                            CVUtil.DrawFocusRectangle(frame, rect, 50, color, 10);
                            Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 2, frame.Height / 2));
                            Dispatcher.Invoke(() =>
                            {
                                var bitmapSource = frame.ToBitmapSource();
                                FaceImage.Source = bitmapSource;
                            });
                        }

                        if (isVerify)
                        {
                            if (ViewModel.FaceRegistration(frame))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    DialogHost.Close(BaseConstant.BaseDialog);
                                    DialogHost.Close(BaseConstant.RootDialog);
                                    SnackbarService.ShowSuccess("人脸信息录入成功！");
                                });
                                FaceRecognitionService.LoadData();
                                return;
                            }

                            isReady = false;
                            Dispatcher.Invoke(() =>
                            {
                                DialogHost.Close(BaseConstant.RootDialog);
                                SnackbarService.ShowError("人脸信息录入失败！");
                                LoadCameraReader();
                            });
                            return;

                        }

                    }
                }
                finally
                {
                    processSemaphore.Release();
                }
            });
        }

        /// <summary>
        /// The UnloadCameraReader
        /// </summary>
        public void UnloadCameraReader()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        /// <summary>
        /// The Close_Button_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="System.Windows.RoutedEventArgs"/></param>
        private void Close_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UnloadCameraReader();
            DialogHost.Close(BaseConstant.BaseDialog);
        }
    }
}
