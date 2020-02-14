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

        public SSIDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add index and unique constraints
            modelBuilder.Entity<Hospital>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<InsuranceCo>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<InsuranceCo> InsuranceCompanies { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentSignedMessage> DocumentSignedMessages { get; set; }
    }
}
