using BaseApp.Core.Domain;
using BaseApp.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Core.Db
{
    public class BaseDbContext : DbContext
    {

        public DbSet<SysUser> Users { get; set; }
        public DbSet<SysMenu> Menus { get; set; }
        public DbSet<SysRole> Roles { get; set; }
        public DbSet<SysRoleMenu> roleMenus { get; set; }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>(e =>
            {
                e.Property(e => e.LockFlag).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });

            modelBuilder.Entity<SysMenu>(e =>
            {
                e.Property(e => e.Position).HasConversion(v => v.ToString(), v => (MenuPositionEnum)Enum.Parse(typeof(MenuPositionEnum), v));
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
