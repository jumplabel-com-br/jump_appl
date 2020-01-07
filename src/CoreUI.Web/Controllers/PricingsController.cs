using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Hosting;
using CoreUI.Web.Controllers.APIs;
using MySql.Data.MySqlClient;
using System.Data;
using CoreUI.Web.Models.ViewModel;

namespace CoreUI.Web.Controllers
{
    public class PricingsController : Controller
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

        public PricingsController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, PricingService pricing, Files files, IConfiguration config, IHostingEnvironment env)
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

        // GET: Pricings
        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            var listPricing = await _pricingService.FindAllAsync();
            var clientes = await _clientService.FindAllAsync(accessLevel, empId);
            var managers = await _employeeService.FindAllManagersAsync();
            var funcionarios = await _employeeService.FindAllAsync();
            var typePricing = await _context.TypePricing.ToListAsync();

            var viewModel = new PricingFormViewModel { ListPricing = listPricing, Clients = clientes, Employees = funcionarios, Managers = managers, TypePricing = typePricing };
            return View(viewModel);
        }

        // GET: Pricings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            var pricing = await _context.Pricing
                .FirstOrDefaultAsync(m => m.Id == id);

            var clientes = await _clientService.FindAllAsync(accessLevel, empId);
            var managers = await _employeeService.FindAllManagersAsync();
            var funcionarios = await _employeeService.FindAllAsync();
            var typePricing = await _context.TypePricing.ToListAsync();

            var viewModel = new PricingFormViewModel { Pricing = pricing, Clients = clientes, Employees = funcionarios, Managers = managers, TypePricing = typePricing };
            if (pricing == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Pricings/Create
        public async Task<IActionResult> Create()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            var listPricing = await _pricingService.FindAllAsync();
            var clientes = await _clientService.FindAllAsync(accessLevel, empId);
            var managers = await _employeeService.FindAllManagersAsync();
            var funcionarios = await _employeeService.FindEmployeesActivesAsync();
            var typePricing = await _context.TypePricing.ToListAsync();

            var viewModel = new PricingFormViewModel { ListPricing = listPricing, Clients = clientes, Employees = funcionarios, Managers = managers, TypePricing = typePricing };
            return View(viewModel);
        }

        // POST: Pricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateAsync")]
        [ValidateAntiForgeryToken]
        public async Task<int> Create(Pricing pricing)
        {

            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }


            if (ModelState.IsValid)
            {
                _context.Add(pricing);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //return View(pricing);

            
            string queryString = "SELECT id FROM Pricing order by id desc limit 1;";
            var dt = ReturnDataAdapter(queryString);
            int lastId = (int)dt.Rows[0]["id"] == null ? 1 : (int)dt.Rows[0]["id"];

            return lastId;
        }

        // GET: Pricings/Edit/5
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

            GetSessions();

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            var pricing = await _context.Pricing.FindAsync(id);
            var clientes = await _clientService.FindAllAsync(accessLevel, empId);
            var managers = await _employeeService.FindAllManagersAsync();
            var funcionarios = await _employeeService.FindEmployeesActivesAsync();
            var typePricing = await _context.TypePricing.ToListAsync();

            var viewModel = new PricingFormViewModel { Pricing = pricing, Clients = clientes, Employees = funcionarios, Managers = managers, TypePricing = typePricing };
            if (pricing == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Pricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("EditAsync")]
        [ValidateAntiForgeryToken]
        public async Task<int> Edit(int id, Pricing pricing)
        {
            /*
            if (id != pricing.Id)
            {
                return NotFound();
            }
            */

            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    /*
                    if (!PricingExists(pricing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                    */
                }
                //return RedirectToAction(nameof(Index));
            }
            //return View(pricing);
            return id;
        }

        // GET: Pricings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.Pricing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pricing == null)
            {
                return NotFound();
            }

            return View(pricing);
        }

        // POST: Pricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var pricing = await _context.Pricing.FindAsync(id);
            _context.Pricing.Remove(pricing);

            var detailsPricing = await _context.DetailsPricing.Where(x => x.Pricing_Id == id).FirstOrDefaultAsync();
            if (detailsPricing != null)
            {
                _context.DetailsPricing.Remove(detailsPricing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        private bool PricingExists(int id)
        {
            return _context.Pricing.Any(e => e.Id == id);
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }

        public DataTable ReturnDataAdapter(string queryString)
        {
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = command;

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
            
        }

        public IActionResult ExpiredSession()
        {
            HttpContext.Session.SetString(SessionExpired, "true");
            return RedirectToAction("Index", "Home", "Index");
        }
    }
}
