using System;
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
        private readonly IConfiguration _config;
        IHostingEnvironment _appEnvironment;

        public ModeAdminController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, ClientService client, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
            _clientService = client;
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
        const string storage = "Hour\\";

        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            try
            {
                var result = await _hourService.FindAllAsync();
                return View(result);

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /* GET: Hours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hour.ToListAsync());
        }*/

        // GET: Hours/Details/5
        public async Task<IActionResult> Details(int? id, int Employee_Id)
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

        // GET: Hours/Create
        public async Task<IActionResult> Create()
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

                var clients = await _clientService.FindAllAsync();
                var projects = await _projectService.FindAllAsync();
                var employees = await _employeeService.FindAllAsync();
                //var projectsTeam = await _projectTeamService.FindAllAsync();
                var viewModel = new HourFormViewModel { Projects = projects, Employees = employees, Clients = clients};
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        // POST: Hours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hour hour, IFormFile Document)
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
                    var clients = await _clientService.FindAllAsync();
                    var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                    var employees = await _employeeService.FindAllAsync();
                    var viewModel = new HourFormViewModel { Projects = projects, Employees = employees, Clients = clients };

                    return View(viewModel);
                }

                if (Document != null)
                {
                    int id = (from result in _context.Hour
                              orderby result.Id descending
                              select result).First().Id + 1;


                    EnviarArquivo(Document, id, storage);
                }

                await _hourService.InsertAsync(hour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }


            /*
            if (ModelState.IsValid)
            {
                _context.Add(hour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hour);
            */
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


            /*
            if (hour == null)
            {
                return NotFound();
            }

            return View(hour);

           var obj = await _hourService.FindByIdAsync(id.Value);
           if (obj == null)
           {
               return RedirectToAction(nameof(Error), new { message = "Id not found" });
           }

           List<Project> projects = await _projectService.FindAllAsync();
           HourFormViewModel viewModel = new HourFormViewModel { Hour = obj, Projects = projects };
           return View(viewModel);*/
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
                            EnviarArquivo(Document, hour.Id, storage);
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
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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

                throw new DbConcurrencyException(e.Message);
            }
        }

        public async Task<IActionResult> ModeAdmin()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }


            try
            {
                var result = await _hourService.FindAllAsync();
                return View("ModeAdmin", result);

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
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
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            connection.Close();
        }

        public async Task<IActionResult> UpdateStatus(int status, string ids)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            int id = ViewBag.Id;
            string queryString = "update Hour set Approver = " + id + ", Approval = '" + status + "' where Id in (" + ids + ")";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            connection.Close();

            return RedirectToAction(nameof(Index));
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

        public async void EnviarArquivo(IFormFile Document, dynamic nameId, string storage)
        {

            // < define a pasta onde vamos salvar os arquivos >
            string pasta = "Files";
            // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
            //string nomeArquivo = DateTime.Now.ToString().Replace('/','-').Replace(':', '&').Replace(" ", "") + "_" + id + "_" + Document.FileName;
            string nomeArquivo;
            if (Document.FileName != "" && Document.FileName != null)
            {
                nomeArquivo = nameId + "-";
                nomeArquivo += Document
                    .FileName
                    .Replace(" ", "")
                    .Replace("&", "")
                    .Replace("@", "")
                    .Replace("#", "")
                    .Replace("$", "")
                    .Replace("%", "")
                    .Replace("*", "");
            }
            else
            {
                nomeArquivo = "Sem Documento";
            }


            //< obtém o caminho físico da pasta wwwroot >
            string caminho_WebRoot = _appEnvironment.WebRootPath;
            // monta o caminho onde vamos salvar o arquivo : 
            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
            string caminhoDestinoArquivo = caminho_WebRoot + "\\" + pasta + "\\";
            // incluir a pasta Recebidos e o nome do arquivo enviado : 
            // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
            string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + storage + nomeArquivo;
            //copia o arquivo para o local de destino original
            using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
            {
                await Document.CopyToAsync(stream);
            }
        }
    }
}
