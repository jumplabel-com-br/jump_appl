using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using Microsoft.AspNetCore.Hosting;
using CoreUI.Web.Services;
using CoreUI.Web.Models.ViewModel;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CoreUI.Web.Controllers
{
    public class Project_teamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly HourService _hourService;
        private readonly AccessLevelService _accessLevelService;
        private readonly ClientService _clientService;
        IHostingEnvironment _appEnvironment;
        private readonly IConfiguration _config;


        public Project_teamController(ApplicationDbContext context, ProjectService project, ProjectTeamService projectTeam, EmployeeService employee, HourService hour, AccessLevelService accessLevel, ClientService client, IHostingEnvironment env, IConfiguration config)
        {
            _context = context;
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _accessLevelService = accessLevel;
            _appEnvironment = env;
            _clientService = client;
            _config = config;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        const string SessionImgLogo = "false";
        //Files Files;

        // GET: Project_team
        public async Task<IActionResult> Index(int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                //ViewBag.Month = month;
                //ViewBag.Year = year;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;
                int accessLevel = ViewBag.AcessLevel;
                int employeeId = ViewBag.Id;

                var projectTeam = await _projectTeamService.FindAllAsync(accessLevel, employeeId, clients, projects, employees);
                var clientes = await _clientService.FindAllAsync(accessLevel, employeeId);
                var projetos = await _projectService.FindProjectAsync(accessLevel, employeeId);
                var funcionarios = await _employeeService.FindAllAsync();
                var viewModel = new ProjectTeamFormViewModel { ProjectsTeams = projectTeam, Client = clientes, Project = projetos, Employee = funcionarios };

                return View(viewModel);
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }


            //return View(await _context.Project_team.ToListAsync());
            //return View(projectTeam);
        }

        // GET: Project_team/Details/5
        public async Task<IActionResult> Details(int? id, int? clients, int? projects, int? employees)
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

                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;

                var project_team = await _context.Project_team.FindAsync(id);
                var funcionarios = await _employeeService.FindEmployeeAsync(employees);
                var projetos = await _projectService.FindProjectAsync(projects);
                var clientes = await _clientService.FindClientAsync(clients);
                var viewModel = new ProjectTeamFormViewModel { Project = projetos, Employee = funcionarios, Client = clientes, Project_team = project_team };

                if (project_team == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }


        }

        // GET: Project_team/Create
        public async Task<IActionResult> Create(int? clients, int? projects, int? employees)
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
                ViewBag.Employees = employees;

                var employee = await _employeeService.FindAllAsync();
                var project = await _projectService.FindAllAsync();
                var client = await _clientService.FindAllAsync(accessLevel, employeeId);
                var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Client = client };

                return View(viewModel);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }


        }

        // POST: Project_team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project_team project_team, int? clients, int? projects, int? employees)
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
                    _context.Add(project_team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { clients, projects, employees});
                }
                return View(project_team);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }


        }

        // GET: Project_team/Edit/5
        public async Task<IActionResult> Edit(int? id, int? clients, int? projects, int? employees)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;

                if (id == null)
                {
                    return NotFound();
                }

                var project_team = await _context.Project_team.FindAsync(id);
                var employee = await _employeeService.FindEmployeeAsync(employees);
                var project = await _projectService.FindProjectAsync(projects);
                var client = await _clientService.FindClientAsync(clients);
                var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Project_team = project_team, Client = client };

                if (project_team == null)
                {
                    return NotFound();
                }
                return View(viewModel);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }
        }

        // POST: Project_team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Project_Id,Employee_Id,Start_Date,End_Date")] Project_team project_team, int? clients, int? projects, int? employees)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                if (id != project_team.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(project_team);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        if (!Project_teamExists(project_team.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            return RedirectToAction(nameof(Error), new {message = e.Message});
                        }
                    }
                    return RedirectToAction(nameof(Index), new { clients, projects, employees});
                }
                return View(project_team);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }


        }

        // GET: Project_team/Delete/5
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

                var project_team = await _context.Project_team
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (project_team == null)
                {
                    return NotFound();
                }

                return View(project_team);
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
            }


        }

        // POST: Project_team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? clients, int? projects, int? employees)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var project_team = await _context.Project_team.FindAsync(id);
                _context.Project_team.Remove(project_team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {clients, projects, employees});
            }
             catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new {message = e.Message});
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

        private bool Project_teamExists(int id)
        {
            return _context.Project_team.Any(e => e.Id == id);
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
