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
using MaterialDemo.ViewModels.Pages.Business.VObject;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace BaseApp.Business.ViewModels
{
    public partial class BusinessIndexViewModel : ObservableObject, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(BusinessIndexViewModel));
        
        private readonly IUnitOfWork<BusinessDbContext> business_unitOfWork;

        public BusinessIndexViewModel(IUnitOfWork<BaseDbContext> base_unitOfWork, IUnitOfWork<BusinessDbContext> business_unitOfWork)
        {
            this.business_unitOfWork = business_unitOfWork;
        }

        #region field
        [ObservableProperty]
        private ObservableCollection<BusinessIndexItem> _Items_9 = new();
        #endregion


        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            Task.Run(() =>
            {
                BusinessIndexItem item = new() {
                    LocationId = 1,
                    LocationInfo = "1层A01柜",
                    LSlotStatus = SlotStatusEnums.CHARGE,
                    RSlotStatus = SlotStatusEnums.FULL
                };
                Items_9 = [item];
            });
        }
    }
}
