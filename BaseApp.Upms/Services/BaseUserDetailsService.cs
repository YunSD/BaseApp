using BaseApp.Core.Db;
using BaseApp.Core.Domain;
using BaseApp.Core.Security;
using BaseApp.Core.UnitOfWork;

namespace BaseApp.Upms.Services
{
    public class BaseUserDetailsService
    {

        private IUnitOfWork _unitOfWork;

        public BaseUserDetailsService(IUnitOfWork<BaseDbContext> unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public SecurityUser LoadSecurityUser(SysUser user)
        {

            string? roleName = default;
            List<SysMenu>? menus = default;

            if (user.RoleId != null)
            {
                SysRole role = _unitOfWork.GetRepository<SysRole>().Find(user.RoleId);
                if (role != null) roleName = role.Name;

                List<SysRoleMenu> roleMenus = _unitOfWork.GetRepository<SysRoleMenu>().GetAll(predicate: e => e.RoleId == user.RoleId).ToList();
                if (roleMenus.Any())
                {
                    menus = _unitOfWork.GetRepository<SysMenu>().GetAll(predicate: e => roleMenus.Select(e => e.MenuId)
                        .Contains(e.MenuId)).ToList();
                }
            }

            return new SecurityUser(user, roleName, menus);
        }
    }
}
