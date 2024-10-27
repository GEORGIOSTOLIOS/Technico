using Microsoft.EntityFrameworkCore;
using Technico.Models;

namespace Technico.Data
{
    public class TechnicoDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        
        public TechnicoDbContext(DbContextOptions<TechnicoDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Owner>()
                .HasIndex(o => o.VatNumber)
                .IsUnique();

            modelBuilder
                .Entity<Property>()
                .HasIndex(p => p.IdentificationNumber)
                .IsUnique();
        }
    }
}