using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Models.ViewModel;
using CoreUI.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace CoreUI.Web.Controllers
{
    public class DetailsPricingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly ClientService _clientService;
        private readonly PricingService _pricingService;
        private readonly Files _files;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _appEnvironment;


        public DetailsPricingsController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, PricingService pricing, Files files, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _clientService = client;
            _pricingService = pricing;
            _files = files;
            _config = config;
            _appEnvironment = env;
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

            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { DetailsPricing = detailsPricing, Hiring = hiring };
            if (detailsPricing == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: DetailsPricings/Create
        public async Task<IActionResult> Create()
        {
            var detailsPricing = _pricingService.DetailsPricing();
            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { Hiring = hiring };
            return View(viewModel);
        }

        // POST: DetailsPricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task CreateAsync(DetailsPricing detailsPricing)
        {

            if (ModelState.IsValid)
            {
                _context.Add(detailsPricing);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index", "Pricings");
            }
            //return View(detailsPricing);
        }

        // GET: DetailsPricings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var detailsPricings = await _context.DetailsPricing.FindAsync(id);
            var detailsPricing = await _context.DetailsPricing.Where(x => x.Hiring_Id == id).FirstOrDefaultAsync();

            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { DetailsPricing = detailsPricing, Hiring = hiring };

            if (viewModel == null)
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

        // POST: DetailsPricings/Delete/5
        public async Task Delete(int id)
        {
            var detailsPricing = await _context.DetailsPricing.FindAsync(id);
            _context.DetailsPricing.Remove(detailsPricing);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            //return 0;
        }

        private bool DetailsPricingExists(int id)
        {
            return _context.DetailsPricing.Any(e => e.Id == id);
        }
    }
}
