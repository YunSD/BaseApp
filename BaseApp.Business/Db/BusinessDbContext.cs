using BaseApp.Business.Domain;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Business.Db
{
    public class BusinessDbContext : DbContext
    {

        public DbSet<BusinessBox> BusinessBoxes { get; set; }
        public DbSet<BusinessLocation> businessLocations { get; set; }
        public DbSet<BusinessLocationSlot> businessLocationSlots { get; set; }

        public BusinessDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
