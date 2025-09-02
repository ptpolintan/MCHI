using MHCI.Domain.Entities;
using MHCI.Infrastructure.Persistence.CheckIns;
using Microsoft.EntityFrameworkCore;

namespace MHCI.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<CheckIn> Checkins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CheckInConfiguration());
        }
    }
}
