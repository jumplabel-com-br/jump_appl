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
using Microsoft.AspNetCore.Http;
using CoreUI.Web.Models.ViewModel;
using System.IO;
using System.Diagnostics;
using CoreUI.Web.Services.Exceptions;

namespace CoreUI.Web.Controllers
{
    public class OutlaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly HourService _hourService;
        private readonly AccessLevelService _accessLevelService;
        private readonly ClientService _clienteService;
        private readonly OutlaysService _outlaysService;
        private readonly Files _files;
        IHostingEnvironment _appEnvironment;

        public OutlaysController(ApplicationDbContext context, EmployeeService employeeService, ProjectService projectService, HourService hourService, AccessLevelService accessLevelService, ClientService clientService, OutlaysService outlaysService, Files files, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _employeeService = employeeService;
            _projectService = projectService;
            _hourService = hourService;
            _accessLevelService = accessLevelService;
            _clienteService = clientService;
            _outlaysService = outlaysService;
            _files = files;
            _appEnvironment = appEnvironment;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        const string SessionImgLogo = "false";
        public string storage = "Outlays\\";

        // GET: Outlays
        public async Task<IActionResult> Index(int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int empId = ViewBag.Id;
            var accessLevel = ViewBag.AcessLevel;

            var despesas = await _outlaysService.FindAllAsync(empId,status, clients, projects, employees, month, year);
            var clientes = await _clienteService.FindAllAsync(accessLevel, empId);
            var projetos = await _projectService.FindProjectAsync(empId, accessLevel);
            var funcionarios = await _employeeService.FindAllAsync();
            var viewModel = new OutlaysFormViewModel { Outlay = despesas, Projects = projetos, Employees = funcionarios, Clients = clientes };

            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Status = status;
            ViewBag.Clients = clients;
            ViewBag.Projects = projects;
            ViewBag.Employees = employees;

            return View(viewModel);
        }

        // GET: Outlays/Details/5
        public async Task<IActionResult> Details(int? id)
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

            int AcessLevel = ViewBag.AcessLevel;
            int employeeId = ViewBag.id;

            var outlays = await _context.Outlays
                .FirstOrDefaultAsync(m => m.Id == id);
            //var employees = _employeeService.FindAllAsync();
            var projects = await _projectService.FindProjectAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clients = await _clienteService.FindAllAsync(AcessLevel, employeeId);

            var ViewModel = new OutlaysFormViewModel { Clients = clients, Projects = projects, Outlays = outlays };

            if (outlays == null)
            {
                return NotFound();
            }
            return View(ViewModel);
        }

        // GET: Outlays/Create
        public async Task<IActionResult> Create(int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int AcessLevel = ViewBag.AcessLevel;
            int employeeId = ViewBag.id;

            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Status = status;
            ViewBag.Clients = clients;
            ViewBag.Projects = projects;
            ViewBag.Employees = employees;

            //var employees = _employeeService.FindAllAsync();
            var projetos = await _projectService.FindProjectAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clientes = await _clienteService.FindAllAsync(AcessLevel, employeeId);

            var ViewModel = new OutlaysFormViewModel { Clients = clientes, Projects = projetos };
            return View(ViewModel);
        }

        // POST: Outlays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Employee_Id,Project_Id,Client_Id,Date,NoteNumber,NoteValue,Description,File,Status")] Outlays outlays, IFormFile Document, int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                if (ModelState.IsValid && Document != null)
                {
                    string nameFile = outlays.File;
                    outlays.File = string.Empty;
                    string nomeArquivo = string.Empty;

                    _context.Add(outlays);
                    await _context.SaveChangesAsync();


                    int employeeId = ViewBag.Id;

                    var lastId = (from result in _context.Outlays
                                  where result.Employee_Id == employeeId
                                  orderby result.Id descending
                                  select result).FirstOrDefault();

                    if (lastId != null)
                    {
                        int outlaysId = lastId.Id;
                        nomeArquivo = string.Concat(outlaysId, "-", Document.FileName);
                        _files.EnviarArquivo(Document, nomeArquivo, storage);
                    }

                    outlays.File = nomeArquivo;

                    _context.Update(outlays);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { status, clients, projects, employees, month, year});
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!OutlaysExists(outlays.Id))
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }

            catch (DbConcurrencyException e)
            {

                if (!OutlaysExists(outlays.Id))
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }

            catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }


            return View(outlays);
        }

        // GET: Outlays/Edit/5
        public async Task<IActionResult> Edit(int? id, int? status, int? clients, int? projects, int? employees, int? month, int? year)
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

            int accessLevel = ViewBag.AcessLevel;
            int employeeId = ViewBag.Id;

            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Status = status;
            ViewBag.Clients = clients;
            ViewBag.Projects = projects;
            ViewBag.Employees = employees;

            var outlays = await _context.Outlays.FindAsync(id);
            //var employees = _employeeService.FindAllAsync();
            var projetos = await _projectService.FindProjectAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clientes = await _clienteService.FindAllAsync(accessLevel, employeeId);

            var ViewModel = new OutlaysFormViewModel { Clients = clientes, Projects = projetos, Outlays = outlays };

            if (outlays == null)
            {
                return NotFound();
            }
            return View(ViewModel);
        }

        // POST: Outlays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Employee_Id,Project_Id,Client_Id,Date,NoteNumber,NoteValue,Description,File,Status")] Outlays outlays, IFormFile Document, int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            if (id != outlays.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (Document != null)
                    {
                        string nomeArquivo = string.Concat(id, "-", Document.FileName);
                        _files.EnviarArquivo(Document, nomeArquivo, storage);
                    }

                    _context.Update(outlays);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!OutlaysExists(outlays.Id))
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }

            catch (DbConcurrencyException e)
            {

                if (!OutlaysExists(outlays.Id))
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }

            catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index), new { id, status, clients, projects, employees, month, year});
            //return View(outlays);
        }

        // GET: Outlays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var outlays = await _context.Outlays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (outlays == null)
            {
                return NotFound();
            }

            return View(outlays);
        }

        // POST: Outlays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var outlays = await _context.Outlays.FindAsync(id);
            _context.Outlays.Remove(outlays);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { status, clients, projects, employees, month, year});
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

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }

        private bool OutlaysExists(int id)
        {
            return _context.Outlays.Any(e => e.Id == id);
        }
    }
}
