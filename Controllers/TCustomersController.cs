using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnetcore_desktop_app.Models;

namespace dotnetcore_desktop_app.Controllers
{
    public class TCustomersController : Controller
    {
        private readonly DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext _context;

        public TCustomersController(DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext context)
        {
            _context = context;
        }

        // GET: TCustomers
        public async Task<IActionResult> Index()
        {
              return _context.TCustomers != null ? 
                          View(await _context.TCustomers.ToListAsync()) :
                          Problem("Entity set 'DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext.TCustomers'  is null.");
        }
        
        // GET: TCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TCustomers == null)
            {
                return NotFound();
            }

            var tCustomer = await _context.TCustomers
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tCustomer == null)
            {
                return NotFound();
            }

            return View(tCustomer);
        }

        // GET: TCustomers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FId,FName,FPhone,FEmail,FAddress")] TCustomer tCustomer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tCustomer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tCustomer);
        }

        // GET: TCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TCustomers == null)
            {
                return NotFound();
            }

            var tCustomer = await _context.TCustomers.FindAsync(id);
            if (tCustomer == null)
            {
                return NotFound();
            }
            return View(tCustomer);
        }

        // POST: TCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FId,FName,FPhone,FEmail,FAddress")] TCustomer tCustomer)
        {
            if (id != tCustomer.FId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tCustomer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCustomerExists(tCustomer.FId))
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
            return View(tCustomer);
        }

        // GET: TCustomers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TCustomers == null)
            {
                return NotFound();
            }

            var tCustomer = await _context.TCustomers
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tCustomer == null)
            {
                return NotFound();
            }

            return View(tCustomer);
        }

        // POST: TCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TCustomers == null)
            {
                return Problem("Entity set 'DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext.TCustomers'  is null.");
            }
            var tCustomer = await _context.TCustomers.FindAsync(id);
            if (tCustomer != null)
            {
                _context.TCustomers.Remove(tCustomer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TCustomerExists(int id)
        {
          return (_context.TCustomers?.Any(e => e.FId == id)).GetValueOrDefault();
        }
    }
}
