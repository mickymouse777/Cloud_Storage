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
    public class BirdersController : Controller
    {
        private readonly Cloud_StorageContext _context;

        public BirdersController(Cloud_StorageContext context)
        {
            _context = context;
        }

        // GET: Birders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Birder.ToListAsync());
        }

        // GET: Birders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birder = await _context.Birder
                .FirstOrDefaultAsync(m => m.Birder_Id == id);
            if (birder == null)
            {
                return NotFound();
            }

            return View(birder);
        }

        // GET: Birders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Birder_Id,PartitionKey,RowKey,Name,email,password")] Birder birder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(birder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(birder);
        }

        // GET: Birders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birder = await _context.Birder.FindAsync(id);
            if (birder == null)
            {
                return NotFound();
            }
            return View(birder);
        }

        // POST: Birders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Birder_Id,PartitionKey,RowKey,Name,email,password")] Birder birder)
        {
            if (id != birder.Birder_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(birder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirderExists(birder.Birder_Id))
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
            return View(birder);
        }

        // GET: Birders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var birder = await _context.Birder
                .FirstOrDefaultAsync(m => m.Birder_Id == id);
            if (birder == null)
            {
                return NotFound();
            }

            return View(birder);
        }

        // POST: Birders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birder = await _context.Birder.FindAsync(id);
            if (birder != null)
            {
                _context.Birder.Remove(birder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirderExists(int id)
        {
            return _context.Birder.Any(e => e.Birder_Id == id);
        }
    }
}
