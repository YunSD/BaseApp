namespace BaseApp.Upms.ViewModels
{
    using BaseApp.Core.Domain;
    using BaseApp.Core.Security;
    using BaseApp.Core.Security.Messages;
    using BaseApp.Core.UnitOfWork;
    using BaseApp.Core.Utils;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using System;

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {

        private IUnitOfWork _unitOfWork;


        [ObservableProperty]
        public string? username;


        public LoginViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Login(string password)
        {
            SysUser? user = new SysUser();
            bool status = await Task.Delay(1500).ContinueWith((t) =>
            {
                if (Username == null) return false;
                user = LoadUser(Username);
                if (user == null || !SecurityUtil.Verify(password, user.Password)) return false;
                if (user.IsLocked()) return false;
                return true;
            });

            if (status) WeakReferenceMessenger.Default.Send(new LoginCompletedMessage(LoadSecurityUser(user)));
            return status;
        }

        private SysUser LoadUser(string username)
        {
            return _unitOfWork.GetRepository<SysUser>().GetFirstOrDefault(predicate: u => u.Username != null && u.Username.Equals(username));
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
