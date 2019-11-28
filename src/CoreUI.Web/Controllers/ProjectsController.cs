using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Hosting;
using CoreUI.Web.Models.ViewModel;
using Microsoft.AspNetCore.Http;

namespace CoreUI.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly HourService _hourService;
        private readonly AccessLevelService _accessLevelService;
        private readonly ClientService _clientService;
        IHostingEnvironment _appEnvironment;


        public ProjectsController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, AccessLevelService accessLevel, ClientService client, IHostingEnvironment env)
        {
            _context = context;
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _accessLevelService = accessLevel;
            _appEnvironment = env;
            _clientService = client;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        const string SessionImgLogo = "false";

        // GET: Projects
        public async Task<IActionResult> Index(int? accessLevel, int? employeeId)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                return View(await _projectService.FindAllToListAsync(accessLevel, employeeId));
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var project = _projectService.Find(id);
                var client = await _clientService.FindAllAsync();
                var employee = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { Client = client, Project = project, Employee = employee};

                if (project == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }

        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var client = await _clientService.FindAllAsync();
                var employee = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { Client = client, Employee = employee };

                return View(viewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Project_Name,Client_Id,Cost_Center_Id,Active,Project_Manager_Id,Manager_Id")] Project project)
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
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(project);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id, Project projects)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var project = _projectService.Find(id);
                var client = await _clientService.FindAllAsync();
                var employee = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { Client = client, Project = project, Employee = employee };

                if (project == null)
                {
                    return NotFound();
                }
                return View(viewModel);
            }
            catch (Exception e)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Project_Name,Client_Id,Cost_Center_Id,Active,Project_Manager_Id,Manager_Id")] Project project)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (id != project.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(project);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProjectExists(project.Id))
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
                return View(project);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetSessions();
            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var project = await _context.Project
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (project == null)
                {
                    return NotFound();
                }

                return View(project);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var project = await _context.Project.FindAsync(id);
                _context.Project.Remove(project);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
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


        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
