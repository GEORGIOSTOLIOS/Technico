using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Error);
            optionsBuilder.UseSqlServer("Data Source=(local)\\SQLEXPRESS;Initial Catalog=Technico;Integrated Security=True;TrustServerCertificate=True;");
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
            
            modelBuilder.Entity<Property>().HasQueryFilter(p => p.Type != PropertyType.Deactivated);
            modelBuilder.Entity<Repair>().HasQueryFilter(r => r.Status != Status.Deactivated);
        }
    }
}