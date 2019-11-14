using System;
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
        private readonly IConfiguration _config;
        IHostingEnvironment _appEnvironment;

        public HoursController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, ProjectTeamService projectTeam, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _projectService = project;
            _projectTeamService = projectTeam;
            _employeeService = employee;
            _hourService = hour;
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

        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null )
            {
                return ExpiredSession();
            }


            try
            {
                int empId = ViewBag.Id;
                var result = await _hourService.FindAllPerEmployeeAsync(ViewBag.Id);
                return View(result);

            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
            }
        }


        /* GET: Hours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hour.ToListAsync());
        }*/

        // GET: Hours/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            GetSessions();

            if (ViewBag.Email == null )
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

                return View(hour);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
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

                var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                //var projectsTeam = await _projectTeamService.FindAllAsync();
                var viewModel = new HourFormViewModel { Projects = projects };
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
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

            if (ViewBag.Email == null )
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;
                int accessLevel = ViewBag.AcessLevel;
                if (!ModelState.IsValid || _context.Hour.Count(hours => hours.Id_Project == hour.Id_Project && hours.Date == hour.Date && hours.Arrival_Time == hour.Arrival_Time && hours.Exit_Time == hour.Exit_Time) > 0)
                {
                    var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                    var viewModel = new HourFormViewModel { Hour = hour, Projects = projects };

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
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
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
        public async Task<IActionResult> Edit(int? id)
        {
            GetSessions();

            if (ViewBag.Email == null )
            {
                return ExpiredSession();
            }

            try
            {
                int empId = ViewBag.Id;
                var accessLevel = ViewBag.AcessLevel;

                if (id == null)
                {
                    return RedirectToAction(nameof(Error), new { message = "Hora não encontrada, informar o suporte" });
                }

                var hour = await _context.Hour.FindAsync(id);
                var projects = await _projectService.FindPerEmployeeAsync(empId, accessLevel);
                var viewModel = new HourFormViewModel { Hour = hour, Projects = projects };

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
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
                int empId = ViewBag.Id;

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
                            return RedirectToAction(nameof(Error), new { message = "Hora não encontrada para a edição, informar ao suporte" });
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
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, informar ao suporte" });
            }

        }

        // GET: Hours/Delete/5

        // POST: Hours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            GetSessions();

            if (ViewBag.Email == null )
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

                return RedirectToAction(nameof(Error), new { message = "Não foi possível executar a ação de deletar" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro desconhecido, tentar novamente e avisar o suporte" });
            }

        }


        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(Hour hour)
        {

            GetSessions();

            if (ViewBag.Email == null )
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

                return RedirectToAction(nameof(Error), new { message = "Não foi possível executar a ação de salvar"});
            }
        }

        public async Task<IActionResult> ModeAdmin()
        {
            GetSessions();

            if (ViewBag.Email == null )
            {
                return ExpiredSession();
            }


            try
            {
                var result = await _hourService.FindAllAsync();
                return View("ModeAdmin", result);

            }
            catch (DbConcurrencyException e)
            {

                return RedirectToAction(nameof(Error), new { message = "Não foi possível executar a ação de update" });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new { message = "Erro não definido, tentar novamente e avisar o suporte" });
            }
           
        }

        public IActionResult UpdateStatus(int status, string ids)
        {
            GetSessions();

            if (status == null || status == 0 || ids == "")
            {
                return RedirectToAction(nameof(Error));
            }

            if (ViewBag.Email == null )
            {
                return ExpiredSession();
            }


            string queryString = "update Hour set Approval = '"+status+"' where Id in ("+ids+")";

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
