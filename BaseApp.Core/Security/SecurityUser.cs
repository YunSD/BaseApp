
using BaseApp.Core.Domain;

namespace BaseApp.Core.Security
{
    public sealed class SecurityUser
    {
        public readonly long? UserId;
        public readonly string? UserName;
        public readonly string? Name;
        public readonly string? Avatar;
        public readonly string? Phone;
        public readonly string? Email;
        public readonly long? RoleId;

        public readonly string? RoleName;

        public readonly List<SysMenu> menus = new();

        public SecurityUser(SysUser sysUser, string? RoleName, List<SysMenu>? menus)
        {
            UserId = sysUser.UserId;
            RoleId = sysUser.RoleId;
            UserName = sysUser.Username;
            Name = sysUser.Name;
            Avatar = sysUser.Avatar;
            Phone = sysUser.Phone;
            Email = sysUser.Email;

            this.RoleName = RoleName;
            if (menus != null && menus.Any()) this.menus = menus;
        }
    }
}
