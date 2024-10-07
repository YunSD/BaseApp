using BaseApp.Business.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLocationViewPage: INavigableView<BusinessLocationViewModel>
    {
        public BusinessLocationViewModel ViewModel { get; }

        public BusinessLocationViewPage(BusinessLocationViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
