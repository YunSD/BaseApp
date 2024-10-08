using BaseApp.Business.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLocationSlotViewPage : INavigableView<BusinessLocationSlotViewModel>
    {
        public BusinessLocationSlotViewModel ViewModel { get; }

        public BusinessLocationSlotViewPage(BusinessLocationSlotViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
