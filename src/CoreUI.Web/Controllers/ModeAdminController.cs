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
using MySql.Data.MySqlClient;
using CoreUI.Web.Services.Exceptions;
using CoreUI.Web.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using CoreUI.Web.Services;

namespace CoreUI.Web.Controllers
{
    public class ModeAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly IConfiguration _config;

        public ModeAdminController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, IConfiguration config)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _config = config;
        }


        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";

        public async Task<IActionResult> Index()
        {
            GetSessions();
            int empId = ViewBag.Id;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }

            try
            {
                var result = await _hourService.FindAllAsync();
                return View(result);

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /* GET: Hours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hour.ToListAsync());
        }*/

        // GET: Hours/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            GetSessions();

            int empId = ViewBag.Id;
            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }

            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hour = await _context.Hour
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (hour == null)
                {
                    return NotFound();
                }

                return View(hour);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        // GET: Hours/Create
        public async Task<IActionResult> Create()
        {

            GetSessions();

            int empId = ViewBag.Id;
            int accessLevel = ViewBag.AcessLevel;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }

            try
            {
                var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                var employees = await _employeeService.FindAllAsync();
                //var projectsTeam = await _projectTeamService.FindAllAsync();
                var viewModel = new HourFormViewModel { Projects = projects, Employees = employees};
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        // POST: Hours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hour hour)
        {

            GetSessions();

            int empId = ViewBag.Id;
            int accessLevel = ViewBag.AcessLevel;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }


            try
            {
                if (!ModelState.IsValid || _context.Hour.Count(hours => hours.Id_Project == hour.Id_Project && hours.Date == hour.Date && hours.Arrival_Time == hour.Arrival_Time && hours.Exit_Time == hour.Exit_Time) > 0)
                {
                    var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                    var employees = await _employeeService.FindAllAsync();
                    var viewModel = new HourFormViewModel { Projects = projects, Employees = employees };

                    return View(viewModel);
                }

                await _hourService.InsertAsync(hour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }


            /*
            if (ModelState.IsValid)
            {
                _context.Add(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hour);
            */
        }


        // GET: Hours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetSessions();

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }


            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hour = await _context.Hour.FindAsync(id);
                var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                var employees = await _employeeService.FindAllAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projects, Employees = employees };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }


            /*
            if (hour == null)
            {
                return NotFound();
            }

            return View(hour);

           var obj = await _hourService.FindByIdAsync(id.Value);
           if (obj == null)
           {
               return RedirectToAction(nameof(Error), new { message = "Id not found" });
           }

           List<Project> projects = await _projectService.FindAllAsync();
           HourFormViewModel viewModel = new HourFormViewModel { Hour = obj, Projects = projects };
           return View(viewModel);*/
        }

        // POST: Hours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Hour hour)
        {


            GetSessions();

            int empId = ViewBag.Id;
            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        await _hourService.UpdateAsync(hour);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HourExists(hour.Id))
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
                return View(hour);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        // GET: Hours/Delete/5

        // POST: Hours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            GetSessions();

            int empId = ViewBag.Id;
            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }


            try
            {
                var hour = await _context.Hour.FindAsync(id);
                _context.Hour.Remove(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Hours", "Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }


        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(Hour hour)
        {
            GetSessions();

            int empId = ViewBag.Id;
            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }


            try
            {
                _context.Update(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbConcurrencyException e)
            {

                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<IActionResult> ModeAdmin()
        {
            GetSessions();
            int empId = ViewBag.Id;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }


            try
            {
                var result = await _hourService.FindAllAsync();
                return View("ModeAdmin", result);

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> UpdateStatus(int status, string ids)
        {
            GetSessions();
            int empId = ViewBag.Id;

            if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
            {
                return ExpiredSession();
            }

            string queryString = "update dev_jump.Hour set Approval = '" + status + "' where Id in (" + ids + ")";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return RedirectToAction(nameof(Index));
        }

        public void GetSessions()
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.AcessLevel = HttpContext.Session.GetInt32(SessionAcessLevel);
            ViewBag.TotalMessagesBells = HttpContext.Session.GetInt32(SessionTotalBells);
        }

        public IActionResult ExpiredSession()
        {
            HttpContext.Session.SetString(SessionExpired, "true");
            return RedirectToAction("Index", "Home", "Index");
        }

        private bool HourExists(int id)
        {
            return _context.Hour.Any(e => e.Id == id);
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
    }
}
