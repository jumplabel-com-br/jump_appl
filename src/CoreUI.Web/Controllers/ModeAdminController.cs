﻿using System;
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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CoreUI.Web.Controllers
{
    public class ModeAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly ClientService _clientService;
        private readonly Files _files;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _appEnvironment;

        public ModeAdminController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, Files files, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _clientService = client;
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

        public async Task<IActionResult> Index(int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Billing = Selectbilling;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;

                int empId = ViewBag.Id;
                var accessLevel = ViewBag.AcessLevel;
                //var result = await _hourService.FindAllAsync(month, year);
                var horas = await _hourService.FindAllAsync(Selectbilling, approval, description, clients, projects, employees, month, year, empId, accessLevel);
                var clientes = await _clientService.FindAllAsync(accessLevel, empId);
                var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
                var funcionarios = await _employeeService.FindAllAsync();
                var Description = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                var status = await _hourService.FindStatus("horas");

                //var funcionarios = await _employeeService.FindAllAsync();
                var viewModel = new HourFormViewModel { Hours = horas, Projects = projetos, Clients = clientes, Employees = funcionarios, Description = Description, Locality = localities, Status = status };

                return View(viewModel);

            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }


        /* GET: Hours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hour.ToListAsync());
        }*/

        // GET: Hours/Details/5
        public async Task<IActionResult> Details(int? id, int Employee_Id, int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year)
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

                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Billing = Selectbilling;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;

                var hour = await _context.Hour.FindAsync(id);
                var clientes = await _clientService.FindAllAsync(id, Employee_Id);
                var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
                var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();

                //var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projetos, Clients = clientes, Employees = funcionarios, Description = descriptions, Locality = localities };

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        // GET: Hours/Create
        public async Task<IActionResult> Create(int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year)
        {

            GetSessions();



            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;
                int accessLevel = ViewBag.AcessLevel;

                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Billing = Selectbilling;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;
                ViewBag.Employees = employees;

                var clientes = await _clientService.FindAllAsync(accessLevel, empId);
                var projetos = await _projectService.FindAllAsync(accessLevel, empId);
                var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                //var projectsTeam = await _projectTeamService.FindAllAsync();
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();

                //var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                var viewModel = new HourFormViewModel { Projects = projetos, Clients = clientes, Employees = funcionarios, Description = descriptions, Locality = localities };

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        // POST: Hours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hour hour, IFormFile Document, int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year)
        {

            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                int empId = ViewBag.Id;
                int accessLevel = ViewBag.AcessLevel;

                if (!ModelState.IsValid || _context.Hour.Count(hours => hours.Id_Project == hour.Id_Project && hours.Date == hour.Date && hours.Arrival_Time == hour.Arrival_Time && hours.Exit_Time == hour.Exit_Time) > 0)
                {
                    var clientes = await _clientService.FindAllAsync(accessLevel, empId);
                    var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
                    var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                    var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                    var viewModel = new HourFormViewModel { Projects = projetos, Employees = funcionarios, Clients = clientes, Locality = localities };
                }

                hour.File = string.Empty;
                string nomeArquivo = string.Empty;

                await _hourService.InsertAsync(hour);
                await _context.SaveChangesAsync();

                if (Document != null)
                {
                    var lastId = (from result in _context.Hour
                                  where result.Employee_Id == empId
                                  orderby result.Id descending
                                  select result).FirstOrDefault();

                    if (lastId != null)
                    {
                        int hoursId = lastId.Id;
                        nomeArquivo = string.Concat(hoursId, "-", Document.FileName);
                        hour.File = nomeArquivo;
                        _files.EnviarArquivo(Document, nomeArquivo, storage);
                    }

                    _context.Update(hour);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index), new { Selectbilling, approval, description, clients, projects, employees, month, year});
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // GET: Hours/Edit/5
        public async Task<IActionResult> Edit(int? id, int Employee_Id)
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
                var clients = await _clientService.FindAllAsync(id, Employee_Id);
                var projects = await _projectService.FindProjectAsync(empId, accessLevel);
                var employees = await _employeeService.FindEmployeesAsync();
                var description = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projects, Employees = employees, Clients = clients, Description = description, Locality = localities };

                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // POST: Hours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Hour hour, IFormFile Document)
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
                    try
                    {
                        if (Document != null)
                        {
                            string nomeArquivo = string.Concat(hour.Id, "-", Document.FileName);
                            _files.EnviarArquivo(Document, nomeArquivo, storage);
                        }

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
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        // GET: Hours/Delete/5

        // POST: Hours/Delete/5
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
                var hour = await _context.Hour.FindAsync(id);
                _context.Hour.Remove(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Hours", "Index");
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }


        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(Hour hour)
        {
            GetSessions();

            if (ViewBag.Email == null)
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

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async void UpdateBilling(int id, int billing)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                ExpiredSession();
            }

            if (billing == 0)
            {
                billing = 1;
            }
            else
            {
                billing = 0;
            }

            string queryString = "update Hour set Billing = " + billing + " where Id = " + id + "";
            ExecuteQuery(queryString);
        }

        public async Task<IActionResult> UpdateStatus(int status, string ids, int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();


            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Billing = Selectbilling;
            ViewBag.Approval = approval;
            ViewBag.Description = description;
            ViewBag.Clients = clients;
            ViewBag.Projects = projects;
            ViewBag.Employees = employees;

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int id = ViewBag.Id;
            string queryString = "update Hour set Approver = " + id + ", Approval = '" + status + "' where Id in (" + ids + ")";
            ExecuteQuery(queryString);

            return RedirectToAction(nameof(Index), new { Selectbilling, approval, description, clients, projects, employees, month, year });
        }

        public async void ExecuteQuery(string queryString)
        {
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");
            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            connection.Close();
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
