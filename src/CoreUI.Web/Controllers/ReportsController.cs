using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreUI.Web.Models;
using CoreUI.Web.Models.ViewModel;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CoreUI.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly ClientService _clientService;
        private readonly OutlaysService _outlaysService;
        private readonly Files _files;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _appEnvironment;

        public ReportsController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, Files files, OutlaysService outlaysService, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _clientService = client;
            _outlaysService = outlaysService;
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

        public async Task<IActionResult> ModeAdmin()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var result = await _hourService.FindAllAsync();
            return View(result);
        }

        public async Task<IActionResult> OutlaysAdmin()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var ViewModel = await _outlaysService.FindAllAsync();

            return View(ViewModel);
        }

        public async Task<IActionResult> DetailsModeAdmin(int? id, int Employee_Id)
        {

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                int empId = Employee_Id;
                var accessLevel = ViewBag.AcessLevel;

                if (id == null)
                {
                    return NotFound();
                }

                var hour = await _context.Hour.FindAsync(id);
                var clients = await _clientService.FindAllAsync();
                var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                var employees = await _employeeService.FindAllAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projects, Employees = employees, Clients = clients };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> DetailsOutlaysAdmin(int? id)
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

            var outlays = await _context.Outlays
                .FirstOrDefaultAsync(m => m.Id == id);
            var employees = await _employeeService.FindAllAsync();
            var projects = await _projectService.FindAllAsync();
            var clients = await _clientService.FindAllAsync();

            var ViewModel = new OutlaysFormViewModel { Clients = clients, Projects = projects, Employees = employees, Outlays = outlays };

            if (outlays == null)
            {
                return NotFound();
            }
            return View(ViewModel);
        }

        public async Task<IActionResult> UpdateStatus(int status, string ids)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int id = ViewBag.Id;
            string queryString = "update Outlays set Status = '" + status + "' where Id in (" + ids + ")";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            connection.Close();

            return RedirectToAction(nameof(OutlaysAdmin));
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