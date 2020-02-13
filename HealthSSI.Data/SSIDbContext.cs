using HealthSSI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthSSI.Data
{
    public class SSIDbContext : DbContext
    {
        public SSIDbContext(DbContextOptions<SSIDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add index and unique constraint on Name
            modelBuilder.Entity<Hospital>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }

        public DbSet<Hospital> Hospitals { get; set; }
    }
}
