using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Data;
using Sirbu_Iulia_Proiect_Restaurante.Models;
using Sirbu_Iulia_Proiect_Restaurante.Models.LibraryViewModels;

namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryContext _context;


        public HomeController(LibraryContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Statistics(string sortOrder)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "date_asc";

            // Detailed statistics grouped by restaurant and date
            var detailedStatistics = from r in _context.Reservation
                                     group r by new { r.Restaurant.Name, r.ReservationDate } into grouped
                                     select new ReservationStatistics
                                     {
                                         RestaurantName = grouped.Key.Name,
                                         ReservationDate = grouped.Key.ReservationDate,
                                         ReservationCount = grouped.Count()
                                     };

            
            switch (sortOrder)
            {
                case "date_desc":
                    detailedStatistics = detailedStatistics.OrderByDescending(s => s.ReservationDate);
                    break;
                case "date_asc":
                    detailedStatistics = detailedStatistics.OrderBy(s => s.ReservationDate);
                    break;
                default:
                    detailedStatistics = detailedStatistics.OrderBy(s => s.ReservationDate);
                    break;
            }

          
            var totalReservations = from r in _context.Reservation
                                    group r by r.Restaurant.Name into grouped
                                    select new ReservationStatistics
                                    {
                                        RestaurantName = grouped.Key,
                                        ReservationCount = grouped.Count()
                                    };

            var viewModel = new StatisticsViewModel
            {
                DetailedStatistics = await detailedStatistics.AsNoTracking().ToListAsync(),
                TotalReservations = await totalReservations.AsNoTracking().ToListAsync()
            };

            return View(viewModel); 
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Notification()
        {
            return View();
        }

        public IActionResult ReservationNotification(int restaurantId)
        {
            var model = new ReservationNotificationViewModel
            {
                RestaurantID = restaurantId
            };
            return View(model);
        }



    }
}
