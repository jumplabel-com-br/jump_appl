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
using Microsoft.AspNetCore.Http;

namespace CoreUI.Web.Controllers
{
    public class PricingDetailsController : Controller
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


        public PricingDetailsController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, PricingService pricing, Files files, IConfiguration config, IHostingEnvironment env)
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

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        const string SessionImgLogo = "false";
        const string storage = "Hour\\";

        // GET: PricingDetails
        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            return View(await _context.PricingDetails.ToListAsync());
        }

        // GET: PricingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            if (id == null)
            {
                return NotFound();
            }

            var pricingDetails = await _context.PricingDetails
                .FirstOrDefaultAsync(m => m.Id == id);

            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { PricingDetails = pricingDetails, Hiring = hiring };
            if (pricingDetails == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: PricingDetails/Create
        public async Task<IActionResult> Create()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var pricingDetails = _pricingService.PricingDetails();
            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { Hiring = hiring };
            return View(viewModel);
        }

        // POST: PricingDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task CreateAsync(PricingDetails pricingDetails)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }

            if (ModelState.IsValid)
            {
                _context.Add(pricingDetails);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index", "Pricings");
            }
            //return View(pricingDetails);
        }

        // GET: PricingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            if (id == null)
            {
                return NotFound();
            }

            //var pricingDetails = await _context.PricingDetails.FindAsync(id);
            var pricingDetails = await _context.PricingDetails.Where(x => x.Id == id).FirstOrDefaultAsync();

            var hiring = await _context.Hiring.ToListAsync();
            var viewModel = new PricingFormViewModel { PricingDetails = pricingDetails, Hiring = hiring };

            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: PricingDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task EditAsync(int id, PricingDetails pricingDetails)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }

            /*if (id != pricingDetails.Id)
            {
                return NotFound();
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricingDetails);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingDetailsExists(pricingDetails.Id))
                    {
                        //return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }

                //return RedirectToAction(nameof(Index), "Pricings");
            }

            //return View(pricingDetails);
        }

        // POST: PricingDetails/Delete/5
        public async Task Delete(int id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }

            var pricingDetails = await _context.PricingDetails.FindAsync(id);
            _context.PricingDetails.Remove(pricingDetails);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            //return 0;
        }

        private bool PricingDetailsExists(int id)
        {
            return _context.PricingDetails.Any(e => e.Id == id);
        }

        public void GetSessions()
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.AcessLevel = HttpContext.Session.GetInt32(SessionAcessLevel);
            ViewBag.TotalMessagesBells = HttpContext.Session.GetInt32(SessionTotalBells);
            ViewBag.SessionImgLogo = HttpContext.Session.GetString(SessionImgLogo);
        }

        public IActionResult ExpiredSession()
        {
            HttpContext.Session.SetString(SessionExpired, "true");
            return RedirectToAction("Index", "Home", "Index");
        }
    }
}
