namespace Sirbu_Iulia_Proiect_Restaurante.Models.LibraryViewModels
{
    public class RestaurantIndexData
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public IEnumerable<Table> Tables { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
