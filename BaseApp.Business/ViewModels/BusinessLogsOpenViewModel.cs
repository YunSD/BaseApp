using BaseApp.Business.Db;
using BaseApp.Business.Domain;
using BaseApp.Business.ViewModels.VO;
using BaseApp.Business.Views;
using BaseApp.Core.Db;
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
    public partial class BusinessLogsOpenViewModel : PageViewModelBase<BusinessLogsOpen>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessLogsOpenViewModel));
        
        private readonly IUnitOfWork<BaseDbContext> base_unitOfWork;
        private readonly IUnitOfWork<BusinessDbContext> business_unitOfWork;
        
        private readonly IRepository<BusinessLogsOpen> repository;

        public BusinessLogsOpenViewModel(IUnitOfWork<BaseDbContext> base_unitOfWork, IUnitOfWork<BusinessDbContext> business_unitOfWork)
        {
            this.base_unitOfWork = base_unitOfWork;
            this.business_unitOfWork = business_unitOfWork;

            repository = business_unitOfWork.GetRepository<BusinessLogsOpen>();
        }

        #region View Field

        [ObservableProperty]
        private string? _searchName;
        [ObservableProperty]
        private string? _searchCode;

        

        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<BusinessLocation, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchCode)) expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode));
            if (!string.IsNullOrWhiteSpace(SearchName)) expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName));

            Func<IQueryable<BusinessLocation>, IOrderedQueryable<BusinessLocation>> orderBy = q => q.OrderBy(u => u.Code);

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
