using BaseApp.Business.ViewModels;
using BaseApp.Core.Utils;
using BaseApp.Resource.Utils;
using System.Windows;
using System.Windows.Controls;

namespace BaseApp.Business.Views
{
    public partial class BusinessGoodsEditorView : UserControl
    {
        public BusinessGoodsEditorViewModel ViewModel { get; }

        public BusinessGoodsEditorView(BusinessGoodsEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();

            // 初始化图片
            BasePageUtil.ShowImageSelector(MaterialImageSelector, ViewModel.Image);
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.ImageSelector)
            {
                Uri imageUri = ((HandyControl.Controls.ImageSelector)sender).Uri;
                if (imageUri != null)
                {
                    // image copy
                    this.ViewModel.Image = BaseFileUtil.UpdateFile(imageUri.LocalPath);
                }
            }
        }
    }
}
