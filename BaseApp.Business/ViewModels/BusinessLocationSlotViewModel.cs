using BaseApp.Business.Db;
using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels.VO;
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
    public partial class BusinessLocationSlotViewModel : PageViewModelBase<BusinessLocationSlotInfo>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessLocationSlotViewModel));
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BusinessLocationSlot> repository;

        private IList<BusinessBox>? BusinessBoxes;
        private IList<BusinessLocation>? BusinessLocations;

        public BusinessLocationSlotViewModel(IUnitOfWork<BusinessDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<BusinessLocationSlot>();

            
        }

        #region View Field

        [ObservableProperty]
        private string? _searchName;
        [ObservableProperty]
        private string? _searchCode;

        

        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<BusinessLocationSlot, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchCode)) expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode));
            if (!string.IsNullOrWhiteSpace(SearchName)) expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName));

            Func<IQueryable<BusinessLocationSlot>, IOrderedQueryable<BusinessLocationSlot>> orderBy = q => q.OrderBy(u => u.Code);

            IPagedList<BusinessLocationSlot> pageList = repository.GetPagedList(predicate: expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);

            var data = pageList.Items.Select(e => {
                BusinessLocationSlotInfo viewInfo = MapperUtil.Map<BusinessLocationSlot, BusinessLocationSlotInfo>(e);
                BusinessBoxes?.Where(box => box.BoxId == viewInfo.BoxId).GetFirstIfPresent(entity => viewInfo.BoxInfo = entity.Name);
                BusinessLocations?.Where(location => location.LocationId == viewInfo.LocationId).GetFirstIfPresent(entity => viewInfo.LocationInfo = entity.Name);
                return viewInfo;
            }).ToList();

            base.RefreshPageInfo(pageList, data);
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
        private async Task OpenEditForm(BusinessLocationSlotInfo? entity)
        {
            BusinessLocationSlotInfo data = new();
            if (entity != null) data = entity;
            
            BusinessLocationSlotEditorViewModel editorViewModel = new BusinessLocationSlotEditorViewModel(data, SubmitEventHandler);
            BusinessLocationSlotEditorView form = new BusinessLocationSlotEditorView(editorViewModel, ServiceProviderUtil.GetRequiredService<BusinessLocationViewModel>());

            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(BusinessLocationSlot entity)
        {

            Expression<Func<BusinessLocationSlot, bool>> pre = p => p.Code == entity.Code && p.SlotId != entity.SlotId;

            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("编码：" + entity.Code + " 不能重复");
                return;
            }

            if (!entity.SlotId.HasValue)
            {
                entity.SlotId = SnowflakeIdWorker.Singleton.nextId();
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
        private async Task DelConfirm(BusinessLocationSlotInfo entity)
        {
            if (!entity.SlotId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog);
            if (!Equals(result, "true")) return;

            // remote
            repository.Delete(entity.SlotId);
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
                IRepository<BusinessBox> box_repository = _unitOfWork.GetRepository<BusinessBox>();
                BusinessBoxes = box_repository.GetAll().ToList();

                IRepository<BusinessLocation> location_repository = _unitOfWork.GetRepository<BusinessLocation>();
                BusinessLocations = location_repository.GetAll().ToList();
                this.OnSearch();
            });
        }
    }
}
