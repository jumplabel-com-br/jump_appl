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
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Dynamic;
using System.Diagnostics;
using CoreUI.Web.Services.Exceptions;


namespace CoreUI.Web.Controllers
{
    public class HoursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;

        public HoursController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour)
        {
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
        }


        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";

        public async Task<IActionResult> Index()
        {
           
            try
            {
                ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
                ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
                ViewBag.Name = HttpContext.Session.GetString(SessionName);
                int empId = ViewBag.Id;
                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                var result = await _hourService.FindAllAsync(ViewBag.Id);
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
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;
                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

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

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }

        }

        // GET: Hours/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;

                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                var projects = await _projectService.FindPerEmployeeAsync();
                var viewModel = new HourFormViewModel { Projects = projects };
                return View(viewModel);
            }
            catch (Exception)
            {
                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");

            }

        }

        // POST: Hours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hour hour)
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);


            try
            {
                int empId = ViewBag.Id;
                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                if (!ModelState.IsValid || _context.Hour.Count(hours => hours.Id_Project == hour.Id_Project && hours.Date == hour.Date && hours.Arrival_Time == hour.Arrival_Time && hours.Exit_Time == hour.Exit_Time) > 0)
                {
                    var projects = await _projectService.FindPerEmployeeAsync();
                    var viewModel = new HourFormViewModel { Hour = hour, Projects = projects };

                    return View(viewModel);
                }

                await _hourService.InsertAsync(hour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
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
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;

                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                if (id == null)
                {
                    return NotFound();
                }

                var hour = await _context.Hour.FindAsync(id);
                var projects = await _projectService.FindPerEmployeeAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projects };

                return View(viewModel);
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
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
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;
                if (ViewBag.Email == null)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(hour);
                        await _context.SaveChangesAsync();
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

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }

        }

        // GET: Hours/Delete/5

        // POST: Hours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;
                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                var hour = await _context.Hour.FindAsync(id);
                _context.Hour.Remove(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Hours", "Index");
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }

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
