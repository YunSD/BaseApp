using BaseApp.App.Services;
using BaseApp.App.Utils;
using OpenCvSharp.WpfExtensions;
using OpenCvSharp;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui;

namespace BaseApp.App.Views
{
    public partial class PersonFaceRegistration : UserControl
    {

        public PersonFaceRegistration()
        {
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
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private int frameCounter = 0;
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (frameCounter++ % 5 != 0) return;

            Task.Run(() =>
            {
                using (OpenCvSharp.Mat frame = CameraService.Read()) {
                    if (!frame.Empty())
                    {
                        FaceUtil.FaceDetect(frame);
                        Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 2, frame.Height / 2));
                        Dispatcher.Invoke(() =>
                        {
                            var bitmapSource = frame.ToBitmapSource();
                            FaceImage.Source = bitmapSource;
                        });
                    }
                }
            });
        }

        public void UnloadCameraReader()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }
    }
}
