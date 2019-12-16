using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;

namespace CoreUI.Web.Controllers
{
    public class TypePricingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypePricingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypePricings
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypePricing.ToListAsync());
        }

        // GET: TypePricings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typePricing = await _context.TypePricing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typePricing == null)
            {
                return NotFound();
            }

            return View(typePricing);
        }

        // GET: TypePricings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypePricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Active")] TypePricing typePricing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typePricing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typePricing);
        }

        // GET: TypePricings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typePricing = await _context.TypePricing.FindAsync(id);
            if (typePricing == null)
            {
                return NotFound();
            }
            return View(typePricing);
        }

        // POST: TypePricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Active")] TypePricing typePricing)
        {
            if (id != typePricing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typePricing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypePricingExists(typePricing.Id))
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
            return View(typePricing);
        }

        // GET: TypePricings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typePricing = await _context.TypePricing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typePricing == null)
            {
                return NotFound();
            }

            return View(typePricing);
        }

        // POST: TypePricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typePricing = await _context.TypePricing.FindAsync(id);
            _context.TypePricing.Remove(typePricing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypePricingExists(int id)
        {
            return _context.TypePricing.Any(e => e.Id == id);
        }
    }
}
