using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Models;

namespace Sirbu_Iulia_Proiect_Restaurante.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Sirbu_Iulia_Proiect_Restaurante.Models.Restaurant> Restaurant { get; set; } = default!;
        public DbSet<Sirbu_Iulia_Proiect_Restaurante.Models.Table> Table { get; set; }
        public DbSet<Sirbu_Iulia_Proiect_Restaurante.Models.Customer> Customer { get; set; }
        public DbSet<Sirbu_Iulia_Proiect_Restaurante.Models.Reservation> Reservation { get; set; }
        public DbSet<Sirbu_Iulia_Proiect_Restaurante.Models.CuisineType> CuisineType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relația dintre Reservation și Table
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Table)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TableID)
                .OnDelete(DeleteBehavior.Restrict); 

            // Relația dintre Reservation și Restaurant
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Restaurant)
                .WithMany(rest => rest.Reservations)
                .HasForeignKey(r => r.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict); 

            // Relația dintre Reservation și Customer
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict); 

            // Relația dintre Restaurant și CuisineType
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.CuisineType)
                .WithMany(ct => ct.Restaurants)
                .HasForeignKey(r => r.CuisineTypeID)
                .OnDelete(DeleteBehavior.SetNull); 
        }


    }
}
    
