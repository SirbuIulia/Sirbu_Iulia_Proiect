using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Data;
using Sirbu_Iulia_Proiect_Restaurante.Hubs;
using Sirbu_Iulia_Proiect_Restaurante.Models;

namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly LibraryContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(LibraryContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize(Roles = "Angajat")]
        public async Task<IActionResult> Index()
        {
            var reservations = _context.Reservation
                .Include(r => r.Customer)
                .Include(r => r.Restaurant)
                .Include(r => r.Table);
            return View(await reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Customer)
                .Include(r => r.Restaurant)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create?restaurantId=...
        public IActionResult Create(int restaurantId)
        {
            var restaurant = _context.Restaurant.Find(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            var tables = _context.Table.Where(t => t.RestaurantID == restaurantId).ToList();

            if (!tables.Any())
            {
                ModelState.AddModelError(string.Empty, "Nu există mese disponibile pentru acest restaurant.");
            }

            ViewData["RestaurantID"] = restaurantId;
            ViewData["RestaurantName"] = restaurant.Name;
            ViewBag.TableID = new SelectList(tables, "ID", "Number");

            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ReservationDate,NumberOfPeople,TableID")] Reservation reservation, int restaurantId)
        {
            var userEmail = User.Identity.Name;
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == userEmail);

            if (customer == null)
            {
                return Unauthorized();
            }

            reservation.CustomerID = customer.CustomerID;
            reservation.RestaurantID = restaurantId;

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();

               
                var hubContext = HttpContext.RequestServices.GetService<IHubContext<ReservationNotificationHub>>();
                if (hubContext != null)
                {
                    var restaurantName = _context.Restaurant.Find(restaurantId)?.Name;
                    var tableNumber = _context.Table.Find(reservation.TableID)?.Number;
                    Console.WriteLine($"Trimitem notificare: Restaurant={restaurantName}, Masă={tableNumber}, Client={customer.Name}, Ora={reservation.ReservationDate}");

                    await hubContext.Clients.Group("Managers").SendAsync("NewReservation", new
                    {
                        RestaurantName = restaurantName,
                        TableNumber = tableNumber,
                        CustomerName = customer.Name,
                        ReservationDate = reservation.ReservationDate.ToString("yyyy-MM-dd HH:mm")
                    });
                }

                return RedirectToAction(nameof(Index));
            }

          
            var restaurant = _context.Restaurant.Find(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            var tables = _context.Table.Where(t => t.RestaurantID == restaurantId).ToList();
            ViewBag.TableID = new SelectList(tables, "ID", "Number", reservation.TableID);
            ViewData["RestaurantID"] = restaurantId;
            ViewData["RestaurantName"] = restaurant.Name;

            return View(reservation);
        }



        // GET: Reservations/Edit/5
        [Authorize(Roles = "Angajat")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var tables = _context.Table.Where(t => t.RestaurantID == reservation.RestaurantID).ToList();
            ViewBag.TableID = new SelectList(tables, "ID", "Number", reservation.TableID);

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ReservationDate,NumberOfPeople,TableID,RestaurantID,CustomerID")] Reservation reservation)
        {
            if (id != reservation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var tables = _context.Table.Where(t => t.RestaurantID == reservation.RestaurantID).ToList();
            ViewBag.TableID = new SelectList(tables, "ID", "Number", reservation.TableID);

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Angajat")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Customer)
                .Include(r => r.Restaurant)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ID == id);
        }
    }
}
