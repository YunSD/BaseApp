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

        public PersonViewPage(PersonViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            BasePageUtil.ShowImageSelector(AvasterImageSelector, ViewModel.Avaster);
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
    }
}
