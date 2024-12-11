using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sirbu_Iulia_Proiect_Restaurante.Models
{
    public class Table
    {
        public int ID { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Numărul mesei trebuie să fie între 1 și 100.")]
        public int Number { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Capacitatea mesei trebuie să fie între 1 și 20.")]
        public int Capacity { get; set; }
        public int RestaurantID { get; set; } 
        public Restaurant? Restaurant { get; set; } 
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
