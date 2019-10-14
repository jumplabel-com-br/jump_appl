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
        //Files Files;

        // GET: Project_team
        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                return View(await _projectTeamService.FindAllAsync());
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }


            //return View(await _context.Project_team.ToListAsync());
            //return View(projectTeam);
        }

        // GET: Project_team/Details/5
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

                var project_team = await _context.Project_team.FindAsync(id);
                var employee = await _employeeService.FindAllAsync();
                var project = await _projectService.FindAllAsync();
                var client = await _clientService.FindAllAsync();
                var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Client = client, Project_team = project_team };

                if (project_team == null)
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

        // GET: Project_team/Create
        public async Task<IActionResult> Create()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var employee = await _employeeService.FindAllAsync();
                var project = await _projectService.FindAllAsync();
                var client = await _clientService.FindAllAsync();
                var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Client = client };

                return View(viewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }


        }

        // POST: Project_team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project_team project_team)
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
                    return RedirectToAction(nameof(Index));
                }
                return View(project_team);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }


        }

        // GET: Project_team/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

                var project_team = await _context.Project_team.FindAsync(id);
                var employee = await _employeeService.FindAllAsync();
                var project = await _projectService.FindAllAsync();
                var client = await _clientService.FindAllAsync();
                var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Project_team = project_team, Client = client };

                if (project_team == null)
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

        // POST: Project_team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Project_Id,Employee_Id,Start_Date,End_Date")] Project_team project_team)
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
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!Project_teamExists(project_team.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            return RedirectToAction("Error", "Home");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(project_team);
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
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
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }


        }

        // POST: Project_team/Delete/5
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
                var project_team = await _context.Project_team.FindAsync(id);
                _context.Project_team.Remove(project_team);
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
    }
}
