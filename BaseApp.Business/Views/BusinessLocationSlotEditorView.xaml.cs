using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels;
using BaseApp.Core.Utils;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLocationSlotEditorView : UserControl
    {
        public BusinessLocationSlotEditorViewModel ViewModel { get; }
        public BusinessLocationViewModel LocationViewModel { get; }

        public BusinessLocationSlotEditorView(BusinessLocationSlotEditorViewModel ViewModel, BusinessLocationViewModel LocationViewModel)
        {
            this.ViewModel = ViewModel;
            this.LocationViewModel = LocationViewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var confirm = new BusinessLocaltionSelectView(LocationViewModel, (row) =>
            {
                if (row != null)
                {
                    ViewModel.BoxId = row.BoxId;
                    ViewModel.LocationId = row.LocationId;
                    ViewModel.LocationInfo = row.Name;
                }
                else
                {
                    ViewModel.BoxId = null;
                    ViewModel.LocationId = null;
                    ViewModel.LocationInfo = null;
                }
            });
            DialogHost.Show(confirm, BaseConstant.RootDialog);
        }
    }
}
