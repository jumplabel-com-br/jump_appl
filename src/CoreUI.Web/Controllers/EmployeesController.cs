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
        private readonly Files _files;
        IHostingEnvironment _appEnvironment;


        public EmployeesController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, AccessLevelService accessLevel, Files files, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _accessLevelService = accessLevel;
            _files = files;
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
        public string storage = "\\Employee\\Document\\";

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
         
                var result = await _employeeService.FindAllAsync();
                return View(result);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

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
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                var accessLevel = await _accessLevelService.FindAllAsync();
                var viewModel = new EmployeeFormViewModel { Access_Level = accessLevel };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Document,Contract_Mode,Active,Appointment,Password,Change_Password,Access_LevelId")] Employee employee, IFormFile Document)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    int empId = ViewBag.Id;

                    //_files.EnviarArquivo(Document, empId, storage);

                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }



        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;

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
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Document,Contract_Mode,Active,Appointment,Password,Change_Password,Access_LevelId")] Employee employee, IFormFile Document)
        {

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;

                if (ModelState.IsValid)
                {

                    //_files.EnviarArquivo(Document, empId, storage);
                    _context.Update(employee);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(employee);
            }

            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
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

        public IActionResult ExpiredSession()
        {
            HttpContext.Session.SetString(SessionExpired, "true");
            return RedirectToAction("Index", "Home", "Index");
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}