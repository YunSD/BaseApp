using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLogsOpenViewPage : INavigableView<BusinessLogsOpenViewModel>
    {
        public BusinessLogsOpenViewModel ViewModel { get; }

        public BusinessLogsOpenViewPage(BusinessLogsOpenViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
