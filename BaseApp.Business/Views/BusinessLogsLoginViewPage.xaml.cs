using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLogsLoginViewPage : INavigableView<BusinessLogsLoginViewModel>
    {
        public BusinessLogsLoginViewModel ViewModel { get; }

        public BusinessLogsLoginViewPage(BusinessLogsLoginViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
