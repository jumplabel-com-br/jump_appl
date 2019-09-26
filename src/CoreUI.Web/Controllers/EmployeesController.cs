using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Http;
using CoreUI.Web.Models.ViewModel;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CoreUI.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly HourService _hourService;
        private readonly AccessLevelService _accessLevelService;
        IHostingEnvironment _appEnvironment;


        public EmployeesController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, AccessLevelService accessLevel, IHostingEnvironment env)
        {
            _context = context;
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _accessLevelService = accessLevel;
            _appEnvironment = env;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        Files Files;

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var result = await _employeeService.FindAllAsync();
            return View(result);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                var accessLevel = await _accessLevelService.FindAllAsync();
                var viewModel = new EmployeeFormViewModel { Access_Level = accessLevel };
                return View(viewModel);
            }
            catch (Exception)
            {
                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Document,Contract_Mode,Active,Salary,Appointment,Password,Change_Password,Access_LevelId")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);

        }

        // GET: Employees/Edit/5
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

                var employee = await _context.Employee.FindAsync(id);
                var accessLevel = await _accessLevelService.FindAllAsync();
                var viewModel = new EmployeeFormViewModel { Employee = employee, Access_Level = accessLevel };
                return View(viewModel);
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Document,Contract_Mode,Active,Salary,Appointment,Password,Change_Password,Access_LevelId")] Employee employee, IFormFile Document)
        {

            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            string storage = "\\Employee\\Document\\";

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
                        Files.EnviarArquivo(Document, empId, storage);
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
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
                return View(employee);
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }

        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}