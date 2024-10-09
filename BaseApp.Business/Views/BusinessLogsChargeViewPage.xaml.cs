using BaseApp.Business.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLogsChargeViewPage : INavigableView<BusinessLogsChargeViewModel>
    {
        public BusinessLogsChargeViewModel ViewModel { get; }

        public BusinessLogsChargeViewPage(BusinessLogsChargeViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
