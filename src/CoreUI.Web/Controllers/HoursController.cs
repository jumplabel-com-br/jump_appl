﻿using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CoreUI.Web.Controllers
{
    public class HoursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ProjectTeamService _projectTeamService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly ClientService _clientService;
        private readonly Files _files;
        private readonly IConfiguration _config;
        IHostingEnvironment _appEnvironment;

        public HoursController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ClientService client, ProjectTeamService projectTeam, Files files, IConfiguration config, IHostingEnvironment env)
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
        const string SessionTypeReleases = "_TypeReleases";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        const string SessionImgLogo = "false";
        const string storage = "Hour\\";
        const string storageAttachment = "Hour\\Attachment\\";

        public async Task<IActionResult> Index(int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                //int empId = ViewBag.Id;
                //var result = await _hourService.FindAllPerEmployeeAsync(ViewBag.Id);

                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Billing = Selectbilling;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;

                int empId = ViewBag.Id;
                var accessLevel = ViewBag.AcessLevel;
                //var result = await _hourService.FindAllAsync(month, year);
                var horas = await _hourService.FindAllPerEmployeeAsync(ViewBag.Id, Selectbilling, approval, description, clients, projects, month, year);
                var clientes = await _clientService.FindAllAsync(accessLevel, empId);
                var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                var status = await _hourService.FindStatus("horas");

                //var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                var viewModel = new HourFormViewModel { Hours = horas, Projects = projetos, Clients = clientes, Description = descriptions, Locality = localities, Status = status };


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
        public async Task<IActionResult> Details(int? id, int? Selectbilling, int? approval, int? description, int? clients, int? projects, int? month, int? year)
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

                var hour = await _context.Hour
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (hour == null)
                {
                    return NotFound();
                }

                int empId = ViewBag.Id;
                var accessLevel = ViewBag.AcessLevel;

                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;

                var horas = await _hourService.FindAllPerEmployeeAsync(ViewBag.Id, Selectbilling, approval, description, clients, projects, month, year);
                var clientes = await _clientService.FindAllAsync(accessLevel, empId);
                var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                //var funcionarios = await _employeeService.FindEmployeesActivesAsync();
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projetos, Clients = clientes, Description = descriptions, Locality = localities };



                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }

        }

        // GET: Hours/Create
        public async Task<IActionResult> Create(int? approval, int? description, int? clients, int? projects, int? month, int? year)
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
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;

                var projetos = await _projectService.FindProjecPerEmployeetAsync(empId);
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();
                var viewModel = new HourFormViewModel { Projects = projetos, Description = descriptions, Locality = localities };
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
        public async Task<IActionResult> Create(Hour hour, IFormFile Document, IFormFile Attachment, int? approval, int? description, int? clients, int? projects, int? month, int? year)
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
                string email = ViewBag.Email;

                if (_context.Employee.Count(elem => elem.TypeReleases != 10 && elem.Email == email) > 0)
                {
                    if (!ModelState.IsValid)
                    {
                        return RedirectToAction(nameof(Error), new { message = "Erro no servidor, não foi possivel carregar todas as informações" });
                    }

                    if (_context.Hour.Count(hours => hours.Id_Project == hour.Id_Project && hours.Date == hour.Date && hours.Arrival_Time == hour.Arrival_Time && hours.Exit_Time == hour.Exit_Time && hours.Employee_Id == empId) > 0)
                    {
                        return RedirectToAction(nameof(Error), new { message = "As informações inseridas estão duplicadas, por favor verificar" });
                    }
                }
                
                if (ModelState.IsValid)
                {
                    hour.File = string.Empty;
                    hour.Attachment = string.Empty;

                    string nomeArquivo = string.Empty;
                    string nomeArquivoAttachment = string.Empty;

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

                    if (Attachment != null)
                    {
                        var lastId = (from result in _context.Hour
                                      where result.Employee_Id == empId
                                      orderby result.Id descending
                                      select result).FirstOrDefault();

                        if (lastId != null)
                        {
                            int hoursId = lastId.Id;
                            nomeArquivoAttachment = string.Concat(hoursId, "-", Attachment.FileName);
                            hour.Attachment = nomeArquivoAttachment;
                            _files.EnviarArquivo(Attachment, nomeArquivoAttachment, storageAttachment);
                        }

                        _context.Update(hour);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = "Informações que foram inseridas não são válidas, por favor verificar" });
                }

                return RedirectToAction(nameof(Index), new { approval, description, clients, projects, month, year });
            }
            catch (NotFoundException e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }


            //return View(hour);
        }

        // GET: Hours/Edit/5
        public async Task<IActionResult> Edit(int? id, int? approval, int? description, int? clients, int? projects, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;
                var accessLevel = ViewBag.AcessLevel;

                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.Approval = approval;
                ViewBag.Description = description;
                ViewBag.Clients = clients;
                ViewBag.Projects = projects;

                if (id == null)
                {
                    return RedirectToAction(nameof(Error), new { message = "Hora não encontrada, informar o suporte" });
                }

                var hour = await _context.Hour.FindAsync(id);
                var projetos = await _projectService.FindProjecPerEmployeetAsync(empId);
                var descriptions = await _context.Description.Where(x => x.Active == 1).ToListAsync();
                var localities = await _context.Locality.Where(x => x.Active == 1).ToListAsync();

                var viewModel = new HourFormViewModel { Hour = hour, Projects = projetos, Description = descriptions, Locality = localities};

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
        public async Task<IActionResult> Edit(Hour hour, IFormFile Document, IFormFile Attachment, int? approval, int? description, int? clients, int? projects, int? month, int? year)
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
                    try
                    {
                        if (Document != null)
                        {
                            string nomeArquivo = string.Concat(hour.Id ,"-",Document.FileName);
                            _files.EnviarArquivo(Document, nomeArquivo, storage);
                            hour.File = nomeArquivo;
                        }

                        if (Attachment != null)
                        {
                            string nomeArquivoAttachment = string.Concat(hour.Id, "-", Attachment.FileName);
                            _files.EnviarArquivo(Attachment, nomeArquivoAttachment, storageAttachment);
                            hour.Attachment = nomeArquivoAttachment;
                        }

                        await _hourService.UpdateAsync(hour);
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        if (!HourExists(hour.Id))
                        {
                            return RedirectToAction(nameof(Error), new { message = e.Message });
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index), new { approval, description, clients, projects, month, year});
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
            catch (DbConcurrencyException e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        public async Task<IActionResult> ModeAdmin(int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var result = await _hourService.FindAllAsync(month, year);
                return View("ModeAdmin", result);

            }
            catch (DbConcurrencyException e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
             catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro não definido, tentar novamente e avisar o suporte" });
            }

        }

        public IActionResult UpdateStatus(int status, string ids)
        {
            GetSessions();

            if (status == null || status == 0 || ids == "")
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar o T.I" });
            }

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            string queryString = "update Hour set Approval = '" + status + "' where Id in (" + ids + ")";

            ExecuteQuery(queryString);

            return RedirectToAction(nameof(ModeAdmin));
        }

        public async void ExecuteQuery(string query)
        {
            string conn = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlCommand command = new MySqlCommand(query, connection);
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
            ViewBag.TypeReleases = HttpContext.Session.GetString(SessionTypeReleases);

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
