using BaseApp.Business.ViewModels;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessBoxViewPage : INavigableView<BusinessBoxViewModel>
    {
        public BusinessBoxViewModel ViewModel { get; }

        public BusinessBoxViewPage(BusinessBoxViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
