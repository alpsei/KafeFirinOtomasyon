using SharedClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace KafeFirinApi.Data
{
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
            .HasOne(u => u.Roles)  // User, bir Role'a sahip
            .WithMany(r => r.Users)  // Role, birden fazla User'a sahip
            .HasForeignKey(u => u.RoleID); // Kullanıcıda Foreign Key olan RoleID

            modelBuilder.Entity<Rates>()
            .HasOne(r => r.Customer)  // Bir Rate, bir Customer'a (User) ait olacak
            .WithMany(u => u.CustomerRates) // Bir Customer, birden fazla Rate'a sahip olabilir
            .HasForeignKey(r => r.CustomerID); // Rates tablosunda CustomerID foreign key olacak

            modelBuilder.Entity<Rates>()
            .HasOne(r => r.Staff)
            .WithMany(u => u.StaffRates)
            .HasForeignKey(r => r.StaffID);

            modelBuilder.Entity<Orders>()
           .HasOne(r => r.Customer)
           .WithMany(u => u.CustomerOrder)
           .HasForeignKey(r => r.CustomerID);

            modelBuilder.Entity<Orders>()
            .HasOne(r => r.Staff)
            .WithMany(u => u.StaffOrder)
            .HasForeignKey(r => r.StaffID);

            modelBuilder.Entity<FeedBacks>()
            .HasOne(r => r.Customer)
            .WithMany(u => u.CustomerFB)
            .HasForeignKey(r => r.CustomerID);

            modelBuilder.Entity<Favorites>()
            .HasOne(r => r.Customer)
            .WithMany(u => u.CustomerFav)
            .HasForeignKey(r => r.CustomerID);
        }
    }
}
