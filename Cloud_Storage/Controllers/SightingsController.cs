using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_Storage.Data;
using Cloud_Storage.Models;

namespace Cloud_Storage.Controllers
{
    public class SightingsController : Controller
    {
        private readonly Cloud_StorageContext _context;

        public SightingsController(Cloud_StorageContext context)
        {
            _context = context;
        }

        // GET: Sightings
        public async Task<IActionResult> Index()
        {
            var cloud_StorageContext = _context.Sighting.Include(s => s.Bird).Include(s => s.Birder);
            return View(await cloud_StorageContext.ToListAsync());
        }

        // GET: Sightings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting
                .Include(s => s.Bird)
                .Include(s => s.Birder)
                .FirstOrDefaultAsync(m => m.Sighting_Id == id);
            if (sighting == null)
            {
                return NotFound();
            }

            return View(sighting);
        }

        // GET: Sightings/Create
        public IActionResult Create()
        {
            ViewData["Bird_Id"] = new SelectList(_context.Bird, "Bird_Id", "Bird_Id");
            ViewData["Birder_Id"] = new SelectList(_context.Birder, "Birder_Id", "Birder_Id");
            return View();
        }

        // POST: Sightings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sighting_Id,Sighting_Date,Sighting_Location,Birder_Id,Bird_Id")] Sighting sighting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sighting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Bird_Id"] = new SelectList(_context.Bird, "Bird_Id", "Bird_Id", sighting.Bird_Id);
            ViewData["Birder_Id"] = new SelectList(_context.Birder, "Birder_Id", "Birder_Id", sighting.Birder_Id);
            return View(sighting);
        }

        // GET: Sightings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting.FindAsync(id);
            if (sighting == null)
            {
                return NotFound();
            }
            ViewData["Bird_Id"] = new SelectList(_context.Bird, "Bird_Id", "Bird_Id", sighting.Bird_Id);
            ViewData["Birder_Id"] = new SelectList(_context.Birder, "Birder_Id", "Birder_Id", sighting.Birder_Id);
            return View(sighting);
        }

        // POST: Sightings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sighting_Id,Sighting_Date,Sighting_Location,Birder_Id,Bird_Id")] Sighting sighting)
        {
            if (id != sighting.Sighting_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sighting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SightingExists(sighting.Sighting_Id))
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
            ViewData["Bird_Id"] = new SelectList(_context.Bird, "Bird_Id", "Bird_Id", sighting.Bird_Id);
            ViewData["Birder_Id"] = new SelectList(_context.Birder, "Birder_Id", "Birder_Id", sighting.Birder_Id);
            return View(sighting);
        }

        // GET: Sightings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting
                .Include(s => s.Bird)
                .Include(s => s.Birder)
                .FirstOrDefaultAsync(m => m.Sighting_Id == id);
            if (sighting == null)
            {
                return NotFound();
            }

            return View(sighting);
        }

        // POST: Sightings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sighting = await _context.Sighting.FindAsync(id);
            if (sighting != null)
            {
                _context.Sighting.Remove(sighting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SightingExists(int id)
        {
            return _context.Sighting.Any(e => e.Sighting_Id == id);
        }
    }
}
