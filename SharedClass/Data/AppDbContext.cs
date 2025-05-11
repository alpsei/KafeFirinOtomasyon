using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Roles> Roles { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Rates> Rates { get; set; }
    public DbSet<FeedBacks> FeedBacks { get; set; }
    public DbSet<Favorites> Favorites { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>()
            .HasOne(u => u.Roles)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rates>()
            .HasOne(r => r.Customer)
            .WithMany(u => u.CustomerRates)
            .HasForeignKey(r => r.CustomerID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Rates>()
            .HasOne(r => r.Staff)
            .WithMany(u => u.StaffRates)
            .HasForeignKey(r => r.StaffID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Customer)
            .WithMany(u => u.CustomerOrder)
            .HasForeignKey(o => o.CustomerID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Staff)
            .WithMany(u => u.StaffOrder)
            .HasForeignKey(o => o.StaffID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<FeedBacks>()
            .HasOne(f => f.Customer)
            .WithMany(u => u.CustomerFB)
            .HasForeignKey(f => f.CustomerID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Favorites>()
            .HasOne(f => f.Customer)
            .WithMany(u => u.CustomerFav)
            .HasForeignKey(f => f.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Products>()
            .HasOne(p => p.ProductCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductID)
            .OnDelete(DeleteBehavior.Restrict);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;User Id=alpersrn;Password=1234;Database=KafeFirinDB;TrustServerCertificate=True;",
                b => b.MigrationsAssembly("KafeFirinApi"));
        }
    }


}
