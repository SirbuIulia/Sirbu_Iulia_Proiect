using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Models;

namespace Sirbu_Iulia_Proiect_Restaurante.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
            {
                if (context.Restaurant.Any())
                {
                    return; 
                }

              
                context.CuisineType.AddRange(
                    new CuisineType { Name = "Italian" },
                    new CuisineType { Name = "Francez" },
                    new CuisineType { Name = "Japonez" }
                );

                context.SaveChanges();

                // Adaugă restaurante
                context.Restaurant.AddRange(
                    new Restaurant { Name = "La Trattoria", Address = "Str. Primăverii, nr. 10", Phone = "0745123456", CuisineTypeID = 1 },
                    new Restaurant { Name = "Bistro Gourmet", Address = "Str. Pădurii, nr. 23", Phone = "0756789123", CuisineTypeID = 2 },
                    new Restaurant { Name = "Sakura Sushi", Address = "Str. Florilor, nr. 45", Phone = "0723345566", CuisineTypeID = 3 }
                );

                context.SaveChanges();

                // Adaugă mese
                context.Table.AddRange(
                    new Table { Number = 1, Capacity = 4, RestaurantID = 1 },
                    new Table { Number = 2, Capacity = 6, RestaurantID = 1 },
                    new Table { Number = 3, Capacity = 2, RestaurantID = 2 },
                    new Table { Number = 4, Capacity = 8, RestaurantID = 3 }
                );

                context.SaveChanges();

                // Adaugă clienți
                context.Customer.AddRange(
                    new Customer { Name = "Ion Popescu", Email = "ion.popescu@example.com", Phone = "0733556677" },
                    new Customer { Name = "Maria Ionescu", Email = "maria.ionescu@example.com", Phone = "0766223344" }
                );

                context.SaveChanges();

                // Adaugă rezervări
                context.Reservation.AddRange(
                    new Reservation
                    {
                        ReservationDate = DateTime.Now.AddDays(1),
                        NumberOfPeople = 4,
                        RestaurantID = 1,
                        TableID = 1,
                        CustomerID = 1
                    },
                    new Reservation
                    {
                        ReservationDate = DateTime.Now.AddDays(2),
                        NumberOfPeople = 2,
                        RestaurantID = 2,
                        TableID = 3,
                        CustomerID = 2
                    }
                );

                context.SaveChanges();
            }
        }
    }
}