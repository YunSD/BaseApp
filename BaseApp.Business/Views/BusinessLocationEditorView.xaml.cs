using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels;
using System.Windows.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessLocationEditorView : UserControl
    {
        public BusinessLocationEditorViewModel ViewModel { get; }

        public BusinessLocationEditorView(BusinessLocationEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();

            // combox index
            if (ViewModel.Boxes != null && ViewModel.BoxId != null)
            {
                int index = ViewModel.Boxes.Select(p => p.BoxId).ToList().IndexOf(ViewModel.BoxId);
                if (index > -1) BoxCombo.SelectedIndex = index;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            var selectedParent = comboBox.SelectedItem as BusinessBox;
            if (selectedParent == null)
            {
                ViewModel.BoxId = 0;
                ViewModel.BoxInfo = null;
                return;
            }
            ViewModel.BoxId = selectedParent.BoxId;
            ViewModel.BoxInfo = selectedParent.Name;
        }
    }
}
