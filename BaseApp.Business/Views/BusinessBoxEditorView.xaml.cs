using BaseApp.Business.ViewModels;
using System.Windows.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessBoxEditorView : UserControl
    {
        public BusinessBoxEditorViewModel ViewModel { get; }
        public BusinessBoxEditorView(BusinessBoxEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
