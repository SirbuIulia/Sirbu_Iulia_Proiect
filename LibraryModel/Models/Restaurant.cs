using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Sirbu_Iulia_Proiect_Restaurante.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        public string Name { get; set; } 
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; } 
        public int? CuisineTypeID { get; set; } 
        public CuisineType? CuisineType { get; set; } 
        public ICollection<Reservation>? Reservations { get; set; } 
        public ICollection<Table>? Tables { get; set; }


    }
}
