using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sirbu_Iulia_Proiect_Restaurante.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z\s]*$", ErrorMessage = "Name must start with an uppercase letter and contain only letters and spaces.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
