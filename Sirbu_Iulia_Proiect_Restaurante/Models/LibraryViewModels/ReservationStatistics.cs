using System;
using System.ComponentModel.DataAnnotations;
namespace Sirbu_Iulia_Proiect_Restaurante.Models.LibraryViewModels
{
    public class ReservationStatistics
    {
        public string RestaurantName { get; set; }
        public int ReservationCount { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
