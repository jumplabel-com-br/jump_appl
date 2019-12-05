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
using System.Diagnostics;

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
        public async Task<IActionResult> Index(int? accessLevel, int? employeeId, int? clients, int? projects, int?  manager_project, int? manager)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                accessLevel = ViewBag.AcessLevel;
                employeeId = ViewBag.Id;

                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Manager = manager;
                ViewBag.Manager_Project = manager_project;

                var Listprojects = await _projectService.FindAllToListAsync(accessLevel, employeeId, clients, projects, manager_project, manager);
                var clientes = await _clientService.FindAllAsync(accessLevel, employeeId);
                var projetos = await _projectService.FindProjectAsync((int)accessLevel, (int)employeeId);
                var funcionarios = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { ListProject = Listprojects, Client = clientes, Projects = projetos, Employee = funcionarios };
                return View(viewModel);
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message});
            }
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id, int? clients, int? projects, int? manager_project, int? manager)
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

                int accessLevel = ViewBag.AcessLevel;
                int employeeId = ViewBag.Id;

                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Manager = manager;
                ViewBag.Manager_Project = manager_project;

                var project = _projectService.Find(id);
                var client = await _clientService.FindAllAsync(accessLevel, employeeId);
                var employee = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { Client = client, Project = project, Employee = employee };

                if (project == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Error));
            }

        }

        // GET: Projects/Create
        public async Task<IActionResult> Create(int? clients, int? projects, int? manager_project, int? manager)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                int accessLevel = ViewBag.AcessLevel;
                int employeeId = ViewBag.Id;

                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Manager = manager;
                ViewBag.Manager_Project = manager_project;

                var client = await _clientService.FindAllAsync(accessLevel, employeeId);
                var employee = await _employeeService.FindAllManagersAsync();
                var viewModel = new ProjectFormViewModel { Client = client, Employee = employee };

                return View(viewModel);
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Error));
            }
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Project_Name,Client_Id,Cost_Center_Id,Active,Project_Manager_Id,Manager_Id")] Project project, int? clients, int? projects, int? manager_project, int? manager)
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
                    return RedirectToAction(nameof(Index), new { clients, projects, manager_project, manager });
                }
                return View(project);
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Error));
            }
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id, int? clients, int? projects, int? manager_project, int? manager)
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

                int accessLevel = ViewBag.AcessLevel;
                int employeeId = ViewBag.Id;

                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Manager = manager;
                ViewBag.Manager_Project = manager_project;

                var project = _projectService.Find(id);
                var client = await _clientService.FindAllAsync(accessLevel, employeeId);
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

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Project_Name,Client_Id,Cost_Center_Id,Active,Project_Manager_Id,Manager_Id")] Project project, int? clients, int? projects, int? manager_project, int? manager)
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
                    return RedirectToAction(nameof(Index), new { clients, projects, manager_project, manager });
                }
                return View(project);
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Error));
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

                return RedirectToAction(nameof(Error));
            }
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? clients, int? projects, int? manager_project, int? manager)
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

                return RedirectToAction(nameof(Index), new { clients, projects, manager_project, manager });
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Error));
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
