using MaterialDemo.Config.Security;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace BaseApp.Upms.ViewModels.Upms
{
    public partial class FolderViewModel : ObservableObject
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenu> repository;


        public FolderViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<SysMenu>();
        }

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private ObservableCollection<object> navigationCards = new();



        public void LoadNavigationItems(INavigationViewItem item)
        {

            // 获取当前角色所有的菜单
            SecurityUser? user = SecurityContext.Singleton.GetUserInfo();
            if (user == null) return;
            List<long?> userMenu = user.menus.Select(m => m.MenuId).ToList();

            string name = (string)item.Content;
            this.Title = name;
            SysMenu curMenu = repository.GetFirstOrDefault(predicate: m => m.Name != null && m.Name.Equals(name));
            if (curMenu == null) return;
            List<SysMenu> childrenMenu = repository.GetAll(predicate: m => m.ParentId == curMenu.MenuId).ToList();

            childrenMenu.Where(m => userMenu.Contains(m.MenuId)).ToList().ForEach(item =>
            {
                NavigationCards.Add(new NavigationCard()
                {
                    Name = item.Name,
                    Icon = BasePageUtil.ParseSymbolIcon(item.Icon),
                    PageType = BasePageUtil.ParseClassType(item.Router),
                    Description = item.Remark,
                });
            });
        }
    }
}
