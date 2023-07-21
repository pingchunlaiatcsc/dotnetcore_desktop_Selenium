using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnetcore_desktop_app.Data;
using dotnetcore_desktop_app.Models;

namespace dotnetcore_desktop_app.Controllers
{
    public class RpgCharatersController : Controller
    {
        private readonly DataContext _context;

        public RpgCharatersController(DataContext context)
        {
            _context = context;
        }

        // GET: RpgCharaters
        public async Task<IActionResult> Index()
        {
              return _context.RpgCharaters != null ? 
                          View(await _context.RpgCharaters.ToListAsync()) :
                          Problem("Entity set 'DataContext.RpgCharaters'  is null.");
        }

        // GET: RpgCharaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RpgCharaters == null)
            {
                return NotFound();
            }

            var rpgCharater = await _context.RpgCharaters
                .FirstOrDefaultAsync(m => m.id == id);
            if (rpgCharater == null)
            {
                return NotFound();
            }

            return View(rpgCharater);
        }

        // GET: RpgCharaters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RpgCharaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,Rpgclass,HitPoints")] RpgCharater rpgCharater)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rpgCharater);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rpgCharater);
        }

        // GET: RpgCharaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RpgCharaters == null)
            {
                return NotFound();
            }

            var rpgCharater = await _context.RpgCharaters.FindAsync(id);
            if (rpgCharater == null)
            {
                return NotFound();
            }
            return View(rpgCharater);
        }

        // POST: RpgCharaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,Rpgclass,HitPoints")] RpgCharater rpgCharater)
        {
            if (id != rpgCharater.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rpgCharater);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RpgCharaterExists(rpgCharater.id))
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
            return View(rpgCharater);
        }

        // GET: RpgCharaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RpgCharaters == null)
            {
                return NotFound();
            }

            var rpgCharater = await _context.RpgCharaters
                .FirstOrDefaultAsync(m => m.id == id);
            if (rpgCharater == null)
            {
                return NotFound();
            }

            return View(rpgCharater);
        }

        // POST: RpgCharaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RpgCharaters == null)
            {
                return Problem("Entity set 'DataContext.RpgCharaters'  is null.");
            }
            var rpgCharater = await _context.RpgCharaters.FindAsync(id);
            if (rpgCharater != null)
            {
                _context.RpgCharaters.Remove(rpgCharater);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RpgCharaterExists(int id)
        {
          return (_context.RpgCharaters?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
