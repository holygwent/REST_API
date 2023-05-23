using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure
{
    public class RestaurantDbContext:DbContext
    {
        private readonly string _connectionString = "Server=DESKTOP-5648FP4;Database=RestaurantDb;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<Restaurant.Domain.ORM.Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant.Domain.ORM.Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);
            modelBuilder.Entity<Address>()
               .Property(r => r.City)
               .IsRequired()
               .HasMaxLength(50);
            modelBuilder.Entity<Address>()
              .Property(r => r.Street)
              .IsRequired()
              .HasMaxLength(50);

            modelBuilder.Entity<Dish>()
                .Property(p => p.Name)
                .IsRequired();
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
