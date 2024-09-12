using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Upms
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
