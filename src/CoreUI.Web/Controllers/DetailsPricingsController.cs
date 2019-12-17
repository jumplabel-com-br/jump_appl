using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Models.ViewModel;

namespace CoreUI.Web.Controllers
{
    public class DetailsPricingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetailsPricingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetailsPricings
        public async Task<IActionResult> Index()
        {
            return View(await _context.DetailsPricing.ToListAsync());
        }

        // GET: DetailsPricings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsPricing = await _context.DetailsPricing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailsPricing == null)
            {
                return NotFound();
            }

            return View(detailsPricing);
        }

        // GET: DetailsPricings/Create
        public async Task<IActionResult> Create()
        {
            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel {  Hiring = hiring };
            return View(viewModel);
        }

        // POST: DetailsPricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsPricing detailsPricing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailsPricing);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Pricings");
            }
            return View(detailsPricing);
        }

        // GET: DetailsPricings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var detailsPricings = await _context.DetailsPricing.FindAsync(id);
            var detailsPricing = await _context.DetailsPricing.Where(x => x.Hiring_Id == id).FirstAsync();

            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { DetailsPricing = detailsPricing, Hiring = hiring };

            if (detailsPricing == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: DetailsPricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DetailsPricing detailsPricing)
        {
            if (id != detailsPricing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailsPricing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailsPricingExists(detailsPricing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Pricings");
            }
            return View(detailsPricing);
        }

        // GET: DetailsPricings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsPricing = await _context.DetailsPricing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailsPricing == null)
            {
                return NotFound();
            }

            return View(detailsPricing);
        }

        // POST: DetailsPricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detailsPricing = await _context.DetailsPricing.FindAsync(id);
            _context.DetailsPricing.Remove(detailsPricing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailsPricingExists(int id)
        {
            return _context.DetailsPricing.Any(e => e.Id == id);
        }
    }
}
