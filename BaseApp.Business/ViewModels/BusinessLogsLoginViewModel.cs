﻿using BaseApp.Business.Db;
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
    public partial class BusinessLogsLoginViewModel : PageViewModelBase<BusinessLogsLogin>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessLogsLoginViewModel));
        
        private readonly IUnitOfWork<BusinessDbContext> business_unitOfWork;
        
        private readonly IRepository<BusinessLogsLogin> repository;

        public BusinessLogsLoginViewModel(IUnitOfWork<BaseDbContext> base_unitOfWork, IUnitOfWork<BusinessDbContext> business_unitOfWork)
        {
            this.business_unitOfWork = business_unitOfWork;

            repository = business_unitOfWork.GetRepository<BusinessLogsLogin>();
        }

        #region View Field

        [ObservableProperty]
        private string? _searchUsername;

        [ObservableProperty]
        private string? _searchName;

        [ObservableProperty]
        private DateTime? _searchStartDate;

        [ObservableProperty]
        private DateTime? _searchEndDate;


        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<BusinessLogsLogin, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchUsername)) expression = expression.MergeAnd(expression, exp => exp.Username != null && exp.Username.Contains(SearchUsername));
            if (!string.IsNullOrWhiteSpace(SearchName)) expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName));
            if (SearchStartDate != null) { expression = expression.MergeAnd(expression, exp => exp.CreateTime != null && exp.CreateTime >= SearchStartDate); }
            if (SearchEndDate != null) { expression = expression.MergeAnd(expression, exp => exp.CreateTime != null && exp.CreateTime <= SearchEndDate.Value.AddDays(1)); }

            Func<IQueryable<BusinessLogsLogin>, IOrderedQueryable<BusinessLogsLogin>> orderBy = q => q.OrderByDescending(u => u.CreateTime);
            
            IPagedList<BusinessLogsLogin> pageList = repository.GetPagedList(predicate: expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);

            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchUsername = null;
            this.SearchName = null;
            this.SearchStartDate = null;
            this.SearchEndDate = null;
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
