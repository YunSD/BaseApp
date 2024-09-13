using BaseApp.Core.Domain;
using BaseApp.Upms.ViewModels;
using BaseApp.Upms.ViewModels.VO;
using System.Windows.Controls;

namespace BaseApp.Upms.Views
{
    public partial class MenuEditorView : UserControl
    {
        public MenuEditorViewModel ViewModel { get; }

        public MenuEditorView(MenuEditorViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();

            // combox index
            if (ViewModel.Parents != null && ViewModel.ParentId != null)
            {
                int index = ViewModel.Parents.Select(p => p.MenuId).ToList().IndexOf(ViewModel.ParentId);
                if (index > -1) ParentCombo.SelectedIndex = index;
            }

            PositionCombo.SelectedIndex = (int)ViewModel.Position;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            var selectedParent = comboBox.SelectedItem as SysMenuViewInfo;
            if (selectedParent == null)
            {
                ViewModel.ParentId = 0;
                ViewModel.ParentName = null;
                return;
            }
            ViewModel.ParentId = selectedParent.MenuId;
            ViewModel.ParentName = selectedParent.Name;
        }

        private void PositionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            ComboBoxItem? selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;
            var selectedParent = selectedItem.Content as string;
            if (selectedParent != null)
            {
                ViewModel.Position = (MenuPositionEnum)Enum.Parse(typeof(MenuPositionEnum), selectedParent);
            }
        }
    }
}
