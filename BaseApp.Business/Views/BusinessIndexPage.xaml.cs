using BaseApp.Business.ViewModels;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessIndexPage : INavigableView<BusinessIndexViewModel>
    {

        public BusinessIndexViewModel ViewModel { get; }

        public BusinessIndexPage(BusinessIndexViewModel viewModel)
        {
            this.ViewModel = viewModel;

            DataContext = this;
            InitializeComponent();
        }
    }
}
