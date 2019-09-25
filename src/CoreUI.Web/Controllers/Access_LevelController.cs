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
    public class Access_LevelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Access_LevelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Access_Level
        public async Task<IActionResult> Index()
        {
            return View(await _context.Access_Level.ToListAsync());
        }

        // GET: Access_Level/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access_Level = await _context.Access_Level
                .FirstOrDefaultAsync(m => m.Id == id);
            if (access_Level == null)
            {
                return NotFound();
            }

            return View(access_Level);
        }

        // GET: Access_Level/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Access_Level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Access_level")] Access_Level access_Level)
        {
            if (ModelState.IsValid)
            {
                _context.Add(access_Level);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(access_Level);
        }

        // GET: Access_Level/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access_Level = await _context.Access_Level.FindAsync(id);
            if (access_Level == null)
            {
                return NotFound();
            }
            return View(access_Level);
        }

        // POST: Access_Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Access_level")] Access_Level access_Level)
        {
            if (id != access_Level.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(access_Level);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Access_LevelExists(access_Level.Id))
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
            return View(access_Level);
        }

        // GET: Access_Level/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access_Level = await _context.Access_Level
                .FirstOrDefaultAsync(m => m.Id == id);
            if (access_Level == null)
            {
                return NotFound();
            }

            return View(access_Level);
        }

        // POST: Access_Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var access_Level = await _context.Access_Level.FindAsync(id);
            _context.Access_Level.Remove(access_Level);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Access_LevelExists(int id)
        {
            return _context.Access_Level.Any(e => e.Id == id);
        }
    }
}
