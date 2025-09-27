using Microsoft.EntityFrameworkCore;
using ReolMarked.Models;

namespace ReolMarked.Data;

public class ReolMarkedContext : DbContext
{
    public DbSet<Reol> Reoler { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=localhost;Database=ReolMarkedDB;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
