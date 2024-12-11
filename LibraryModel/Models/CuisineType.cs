using System.Collections.Generic;
namespace Sirbu_Iulia_Proiect_Restaurante.Models
{
    public class CuisineType
    {
        public int ID { get; set; } 
        public string Name { get; set; } 
        public ICollection<Restaurant>? Restaurants { get; set; }
    }
}
