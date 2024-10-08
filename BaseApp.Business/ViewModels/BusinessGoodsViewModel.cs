using BaseApp.Business.Db;
using BaseApp.Business.Domain;
using BaseApp.Core.Domain;
using BaseApp.Core.UnitOfWork;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BaseApp.Core.Extensions;
using HandyControl.Data;
using log4net;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;
using BaseApp.Core.UnitOfWork.Collections;
using BaseApp.Resource.Controls;
using MaterialDesignThemes.Wpf;
using BaseApp.Core.Utils;
using BaseApp.Business.Views;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessGoodsViewModel : PageViewModelBase<BusinessGoods>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessGoodsViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BusinessGoods> repository;

        public BusinessGoodsViewModel(IUnitOfWork<BusinessDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<BusinessGoods>();
        }


        #region View Field

        [ObservableProperty]
        private string? _searchName;

        [ObservableProperty]
        private string? _searchCode;


        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<BusinessGoods, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchName)) { expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName)); }
            if (!string.IsNullOrWhiteSpace(SearchCode)) { expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode)); }

            Func<IQueryable<BusinessGoods>, IOrderedQueryable<BusinessGoods>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<BusinessGoods> pageList = repository.GetPagedList(predicate: expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchName = null;
            this.SearchCode = null;
            this.OnSearch();
        }

        /// <summary>
        ///     页码改变
        /// </summary>
        [RelayCommand]
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            this.PageIndex = info.Info - 1;
            this.OnSearch();
        }


        #endregion

        #region From Command

        /// <summary>
        /// edit form command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(BusinessGoods? entity)
        {
            BusinessGoods data = new BusinessGoods();
            if (entity != null) data = entity;
            BusinessGoodsEditorViewModel editorViewModel = new BusinessGoodsEditorViewModel(data, SubmitEventHandler);
            var form = new BusinessGoodsEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(BusinessGoods entity)
        {

            Expression<Func<BusinessGoods, bool>> pre = p => p.Code == entity.Code && p.GoodsId != entity.GoodsId;

            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("物料编码：" + entity.Code + " 不能重复");
                return;
            }

            if (!entity.GoodsId.HasValue)
            {
                entity.GoodsId = SnowflakeIdWorker.Singleton.nextId();
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
        private async Task DelConfirm(BusinessGoods entity)
        {
            if (!entity.GoodsId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog);
            if (!Equals(result, "true")) return;

            // remote
            repository.Delete(entity.GoodsId);
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
