﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;

namespace CoreUI.Web.Controllers
{
    public class HiringsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HiringsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hirings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hiring.ToListAsync());
        }

        // GET: Hirings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiring = await _context.Hiring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hiring == null)
            {
                return NotFound();
            }

            return View(hiring);
        }

        // GET: Hirings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hirings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Active")] Hiring hiring)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hiring);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hiring);
        }

        // GET: Hirings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiring = await _context.Hiring.FindAsync(id);
            if (hiring == null)
            {
                return NotFound();
            }
            return View(hiring);
        }

        // POST: Hirings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Active")] Hiring hiring)
        {
            if (id != hiring.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hiring);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HiringExists(hiring.Id))
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
            return View(hiring);
        }

        // GET: Hirings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiring = await _context.Hiring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hiring == null)
            {
                return NotFound();
            }

            return View(hiring);
        }

        // POST: Hirings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hiring = await _context.Hiring.FindAsync(id);
            _context.Hiring.Remove(hiring);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HiringExists(int id)
        {
            return _context.Hiring.Any(e => e.Id == id);
        }
    }
}
