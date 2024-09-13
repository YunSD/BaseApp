using BaseApp.Core.Domain;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using BaseApp.Upms.ViewModels.VO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseApp.Upms.ViewModels
{
    public partial class RoleMenuSelectViewModel : ObservableValidator
    {

        private long? rowId;

        [ObservableProperty]
        private IList<MenuTreeViewInfo> treeInfo;

        [ObservableProperty]
        private IList<SysRoleMenu> select;


        private readonly IUnitOfWork _unitOfWork;



        public RoleMenuSelectViewModel(SysRole entity, IUnitOfWork _unitOfWork)
        {
            rowId = entity.RoleId;
            this._unitOfWork = _unitOfWork;

            Expression<Func<SysRoleMenu, bool>> expression = exp => exp.RoleId != null && exp.RoleId == rowId;
            select = _unitOfWork.GetRepository<SysRoleMenu>().GetAll(predicate: expression).ToList();


            List<SysMenu> allMenus = _unitOfWork.GetRepository<SysMenu>().GetAll().ToList();
            treeInfo = MenuTreeViewInfo.build(allMenus, selected: select.Select(m => m.MenuId).ToList());

        }

        [RelayCommand]
        private void Submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            List<MenuTreeViewInfo> selectedRow = new List<MenuTreeViewInfo>();
            foreach (var item in TreeInfo)
            {
                selectedRow.AddRange(item.GetAllNodes().Where(m => m.IsSelected == true).ToList());
            }

            IRepository<SysRoleMenu> rmRepository = _unitOfWork.GetRepository<SysRoleMenu>();

            // 删除历史数据  
            List<SysRoleMenu> roleMenus = rmRepository.GetAll(predicate: m => m.RoleId == rowId).AsNoTracking().ToList();
            if (roleMenus.Any())
            {
                rmRepository.Delete(roleMenus);
            }

            List<SysRoleMenu> roleMenusToAdd = selectedRow.Select(row =>
            {
                SysRoleMenu rm = new()
                {
                    Id = SnowflakeIdWorker.Singleton.nextId(),
                    RoleId = rowId,
                    MenuId = row.MenuId
                };
                return rm;
            }).ToList();
            if (roleMenusToAdd.Any()) rmRepository.Insert(roleMenusToAdd);
            _unitOfWork.SaveChanges();

            roleMenusToAdd.ForEach(entity =>
            {
                rmRepository.ChangeEntityState(entity, Microsoft.EntityFrameworkCore.EntityState.Detached);
            });

            DialogHost.Close(BaseConstant.BaseDialog);
        }
    }
}
