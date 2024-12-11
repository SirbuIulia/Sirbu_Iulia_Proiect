using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Data;
using Sirbu_Iulia_Proiect_Restaurante.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    [Authorize(Policy = "OnlyAdministrativ")]
    public class TablesController : Controller
    {
        private readonly LibraryContext _context;

        public TablesController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Tables
        public async Task<IActionResult> Index(int? restaurantId)
        {
            if (restaurantId == null)
            {
                
                return View(await _context.Table.Include(t => t.Restaurant).ToListAsync());
            }

           
            var tables = _context.Table.Where(t => t.RestaurantID == restaurantId);
            ViewData["RestaurantName"] = _context.Restaurant.FirstOrDefault(r => r.ID == restaurantId)?.Name;
            ViewData["RestaurantID"] = restaurantId;

            return View(await tables.ToListAsync());
        }

        // GET: Tables/Create
        public IActionResult Create()
        {
           
            ViewData["Restaurants"] = new SelectList(_context.Restaurant, "ID", "Name");
            return View();
        }

        // POST: Tables/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Number,Capacity,RestaurantID")] Table table)
        {
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { restaurantId = table.RestaurantID });
            }

           
            ViewData["Restaurants"] = new SelectList(_context.Restaurant, "ID", "Name", table.RestaurantID);
            return View(table);
        }

        // GET: Tables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

           
            ViewData["Restaurants"] = new SelectList(_context.Restaurant, "ID", "Name", table.RestaurantID);
            return View(table);
        }

        // POST: Tables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Number,Capacity,RestaurantID")] Table table)
        {
            if (id != table.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { restaurantId = table.RestaurantID });
            }

            ViewData["Restaurants"] = new SelectList(_context.Restaurant, "ID", "Name", table.RestaurantID);
            return View(table);
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }


        // GET: Tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Table.FindAsync(id);
            if (table != null)
            {
                _context.Table.Remove(table);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
            return _context.Table.Any(e => e.ID == id);
        }
    }
}
