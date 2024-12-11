using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Data;
using Sirbu_Iulia_Proiect_Restaurante.Models;

namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    [Authorize(Roles = "Angajat")]
    public class CuisineTypesController : Controller
    {
        private readonly LibraryContext _context;

        public CuisineTypesController(LibraryContext context)
        {
            _context = context;
        }

        // GET: CuisineTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CuisineType.ToListAsync());
        }

        // GET: CuisineTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuisineType = await _context.CuisineType
                .Include(c => c.Restaurants) 
                .FirstOrDefaultAsync(m => m.ID == id);

            if (cuisineType == null)
            {
                return NotFound();
            }

            return View(cuisineType);
        }


        // GET: CuisineTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CuisineTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] CuisineType cuisineType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuisineType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuisineType);
        }

        // GET: CuisineTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuisineType = await _context.CuisineType.FindAsync(id);
            if (cuisineType == null)
            {
                return NotFound();
            }
            return View(cuisineType);
        }

        // POST: CuisineTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] CuisineType cuisineType)
        {
            if (id != cuisineType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuisineType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuisineTypeExists(cuisineType.ID))
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
            return View(cuisineType);
        }

        // GET: CuisineTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuisineType = await _context.CuisineType
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cuisineType == null)
            {
                return NotFound();
            }

            return View(cuisineType);
        }

        // POST: CuisineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuisineType = await _context.CuisineType.FindAsync(id);
            if (cuisineType != null)
            {
                _context.CuisineType.Remove(cuisineType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuisineTypeExists(int id)
        {
            return _context.CuisineType.Any(e => e.ID == id);
        }
    }
}
