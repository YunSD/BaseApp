using BaseApp.Business.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Business.Db
{
    public class BusinessDbContext : DbContext
    {

        public DbSet<BusinessBox> BusinessBoxes { get; set; }
        public DbSet<BusinessLocation> BusinessLocations { get; set; }
        public DbSet<BusinessLocationSlot> BusinessLocationSlots { get; set; }
        public DbSet<BusinessGoods> BusinessGoods { get; set; }
        public DbSet<BusinessLogsLogin> BusinessLogsLogins { get; set; }
        public DbSet<BusinessLogsOpen> BusinessLogsOpens { get; set; }
        public DbSet<BusinessLogsCharge> BusinessLogsCharges { get; set; }

        public BusinessDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
