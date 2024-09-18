using BaseApp.App.ViewModels;
using BaseApp.Core.Utils;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.App.Views
{
    public partial class PersonViewPage : INavigableView<PersonViewModel>
    {
        public PersonViewModel ViewModel { get; }

        private OpenCvSharp.VideoCapture videoCapture;

        public PersonViewPage(PersonViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            BasePageUtil.ShowImageSelector(AvasterImageSelector, ViewModel.Avaster);

            InitializeCameraAsync();
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = Password.Password;
            ViewModel.RepeatPassword = RepeatPassword.Password;
            ViewModel.UpdatePasswordCommand.Execute(ViewModel);
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.ImageSelector)
            {
                Uri imageUri = ((HandyControl.Controls.ImageSelector)sender).Uri;
                if (imageUri != null)
                {
                    // image copy
                    this.ViewModel.Avaster = BaseFileUtil.UpdateFile(imageUri.LocalPath);
                }
            }
        }

        /// <summary>
        /// TAB SWITCH
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (UserTabControl.SelectedItem is TabItem selectedTab)
            {
                string header = selectedTab.Header.ToString();
                if ("USER_FACE".Equals(header))
                {
                    LoadCameraReader();
                }
                else
                {
                    UnloadCameraReader();
                }
            }
        }


        private async void InitializeCameraAsync()
        {
            await Task.Run(() =>
            {
                this.videoCapture = new OpenCvSharp.VideoCapture(0);
                this.videoCapture.Open(0);  // 通过摄像头索引打开摄像头

            });
            this.CompositionTarget_Rendering(this, null);
        }

        /// <summary>
        /// 摄像头
        /// </summary>
        private void LoadCameraReader()
        {
            if (!videoCapture.IsOpened())
            {
                SnackbarService.ShowError("摄像头无法正常打开。");
                return;
            }
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void UnloadCameraReader()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private int frameCounter = 0;
        private OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (frameCounter++ % 5 != 0) return;
            if (videoCapture.IsOpened())
            {
                videoCapture.Read(frame);
                Cv2.Resize(frame, frame, new OpenCvSharp.Size(frame.Width / 4, frame.Height / 4));
                if (!frame.Empty())
                {
                    var bitmapSource = frame.ToBitmapSource();
                    FaceImage.Source = bitmapSource;
                }
            }
        }
    }
}
