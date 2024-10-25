using Microsoft.EntityFrameworkCore;
using Technico.Models;

namespace Technico.Data;

public class TechnicoDbContext: DbContext
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=Technico;Integrated Security=True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
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