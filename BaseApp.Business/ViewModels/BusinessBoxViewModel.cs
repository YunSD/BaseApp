using BaseApp.Business.Db;
using BaseApp.Business.Domain;
using BaseApp.Business.Views;
using BaseApp.Core.Domain;
using BaseApp.Core.Extensions;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.UnitOfWork.Collections;
using BaseApp.Core.Utils;
using BaseApp.Resource.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Data;
using log4net;
using MaterialDesignThemes.Wpf;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessBoxViewModel : PageViewModelBase<BusinessBox>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessBoxViewModel));
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BusinessBox> repository;


        public BusinessBoxViewModel(IUnitOfWork<BusinessDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<BusinessBox>();
        }


        #region View Field

        [ObservableProperty]
        private string? _searchName;
        [ObservableProperty]
        private string? _searchCode;

        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<BusinessBox, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchCode)) expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode));
            if (!string.IsNullOrWhiteSpace(SearchName)) expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName));

            Func<IQueryable<BusinessBox>, IOrderedQueryable<BusinessBox>> orderBy = q => q.OrderBy(u => u.Code);

            IPagedList<BusinessBox> pageList = repository.GetPagedList(predicate: expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);

            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchCode = null;
            this.SearchName = null;
            this.OnSearch();
        }

        /// <summary>
        /// 页码改变
        /// </summary>
        [RelayCommand]
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            this.PageIndex = info.Info - 1;
            this.OnSearch();
        }


        #endregion

        #region From Command

        /// <summary>s
        /// edit form command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(BusinessBox? entity)
        {
            BusinessBox data = new();
            if (entity != null) data = entity;
            BusinessBoxEditorViewModel editorViewModel = new BusinessBoxEditorViewModel(data, SubmitEventHandler);
            var form = new BusinessBoxEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(BusinessBox entity)
        {

            Expression<Func<BusinessBox, bool>> pre = p => p.Code == entity.Code && p.BoxId != entity.BoxId;

            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("编号：" + entity.Code + " 不能重复");
                return;
            }

            if (!entity.BoxId.HasValue)
            {
                entity.BoxId = SnowflakeIdWorker.Singleton.nextId();
                repository.Insert(entity);
            }
            else
            {
                repository.Update(entity);
            }

            _unitOfWork.SaveChanges();
            _unitOfWork.TrackClear();
            this.OnSearch();
            DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(BusinessBox entity)
        {
            if (!entity.BoxId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog);
            if (Equals(result, "false")) return;

            // remote
            repository.Delete(entity.BoxId);
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
