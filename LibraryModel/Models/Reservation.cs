using System;
namespace Sirbu_Iulia_Proiect_Restaurante.Models
{
    public class Reservation
    {
        public int ID { get; set; } 
        public DateTime ReservationDate { get; set; } 
        public int NumberOfPeople { get; set; } 
        public int RestaurantID { get; set; } 
        public Restaurant? Restaurant { get; set; } 
        public int TableID { get; set; } 
        public Table? Table { get; set; } 
        public int CustomerID { get; set; } 
        public Customer? Customer { get; set; } 
    }
}
