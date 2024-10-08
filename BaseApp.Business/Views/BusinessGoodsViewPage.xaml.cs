using BaseApp.Business.ViewModels;
using Wpf.Ui.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessGoodsViewPage : INavigableView<BusinessGoodsViewModel>
    {
        public BusinessGoodsViewModel ViewModel { get; }

        public BusinessGoodsViewPage(BusinessGoodsViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
