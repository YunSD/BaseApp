using log4net;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Upms.VObject;
using MaterialDemo.Views.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class MenuViewModel : PageViewModelBase<SysMenuViewInfo>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(MenuViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenu> repository;

        public MenuViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<SysMenu>();
        }


        #region View Field

        [RelayCommand]
        private void OnSearch()
        {
            List<SysMenuViewInfo> menus = repository.GetAll().OrderBy(e => e.Seq).ToList().Select(e => MapperUtil.Map<SysMenu, SysMenuViewInfo>(e)).ToList();
            menus.ForEach(e => { menus.Where(m => m.MenuId == e.ParentId).GetFirstIfPresent(m => e.ParentName = m.Name); });
            base.RefreshPageInfo(menus);
        }

        [RelayCommand]
        private void OnRefresh() => this.OnSearch();


        #endregion

        #region From Command

        /// <summary>
        /// edit form command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(SysMenuViewInfo? entity)
        {
            SysMenuViewInfo data = new SysMenuViewInfo();
            if (entity != null) data = entity;
            MenuEditorViewModel editorViewModel = new MenuEditorViewModel(data, DataList, SubmitEventHandler);
            var form = new MenuEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(SysMenu entity)
        {
            if (!entity.MenuId.HasValue)
            {
                entity.MenuId = SnowflakeIdWorker.Singleton.nextId();
                repository.Insert(entity);
            }
            else
            {
                repository.Update(entity);
            }
            _unitOfWork.SaveChanges();
            repository.ChangeEntityState(entity, Microsoft.EntityFrameworkCore.EntityState.Detached);
            this.OnSearch();
            DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(SysMenuViewInfo entity)
        {
            if (!entity.MenuId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.MenuId;
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog, DeleteRowData);
        }

        // key
        private long? rowId;

        // reference method
        private void DeleteRowData(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "false")) return;
            if (rowId == null) return;
            repository.Delete(rowId);
            _unitOfWork.SaveChanges();

            // 刷新
            this.OnSearch();
        }

        #endregion


        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            Task.Run(() =>
            {
                this.OnSearch();
            });
        }
    }
}
