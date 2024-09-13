using BaseApp.Core.Domain;
using BaseApp.Upms.ViewModels;
using System.Windows.Controls;

namespace BaseApp.Upms.Views
{
    /// <summary>
    /// UserEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UserEditorView : UserControl
    {
        public UserEditorViewModel ViewModel { get; }

        public UserEditorView(UserEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();

            if (ViewModel.RoleId != null)
            {
                int index = ViewModel.Roles.Select(p => p.RoleId).ToList().IndexOf(ViewModel.RoleId);
                if (index > -1) RoleCombo.SelectedIndex = index;
            }
        }

        private void RoleCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            var selected = comboBox.SelectedItem as SysRole;
            if (selected != null)
            {
                ViewModel.RoleId = selected.RoleId;
            }
        }
    }
}
