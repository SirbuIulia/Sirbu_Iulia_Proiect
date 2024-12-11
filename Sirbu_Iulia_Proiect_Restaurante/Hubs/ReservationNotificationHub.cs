using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Sirbu_Iulia_Proiect_Restaurante.Hubs
{
    public class ReservationNotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            Console.WriteLine($"Conexiune stabilită: {Context.ConnectionId}, Utilizator: {userName}");

            if (Context.User.IsInRole("Manager"))
            {
                Console.WriteLine("Utilizatorul este Manager. Adăugăm la grupul Managers.");
                await Groups.AddToGroupAsync(Context.ConnectionId, "Managers");
            }
            else
            {
                Console.WriteLine("Utilizatorul NU are rol de Manager. Conexiunea este refuzată.");
                await Clients.Caller.SendAsync("AccessDenied", "Nu ai acces la notificări.");
                Context.Abort();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Managers");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendReservationNotification(string restaurantName, int tableNumber, string customerName, string reservationDate)
        {
            Console.WriteLine($"Trimit notificare către grupul Managers:");
            Console.WriteLine($"RestaurantName: {restaurantName}, TableNumber: {tableNumber}, CustomerName: {customerName}, ReservationDate: {reservationDate}");

            await Clients.Group("Managers").SendAsync("NewReservation", new
            {
                RestaurantName = restaurantName,
                TableNumber = tableNumber,
                CustomerName = customerName,
                ReservationDate = reservationDate
            });
        }



    }
}
