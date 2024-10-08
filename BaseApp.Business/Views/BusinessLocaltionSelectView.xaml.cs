using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels;
using BaseApp.Core.Domain;
using BaseApp.Core.Utils;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLocaltionSelectView : UserControl
    {
        public BusinessLocationViewModel ViewModel { get; }

        private FormSubmitEventHandler<BusinessLocation> SubmitEvent;

        public BusinessLocaltionSelectView(BusinessLocationViewModel viewModel, FormSubmitEventHandler<BusinessLocation> SubmitEvent)
        {
            this.ViewModel = viewModel;
            this.ViewModel.OnNavigatedTo();
            this.SubmitEvent = SubmitEvent;
            DataContext = this;
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.RootDialog)) return;
            var selectedRow = DataGrid.SelectedItem as BusinessLocation;
            if (selectedRow == null) selectedRow = new();
            SubmitEvent(selectedRow);
            DialogHost.Close(BaseConstant.RootDialog);
        }
    }
}
