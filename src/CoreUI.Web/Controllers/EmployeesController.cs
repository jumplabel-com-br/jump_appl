using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Http;
using CoreUI.Web.Models.ViewModel;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CoreUI.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        private readonly HourService _hourService;
        private readonly AccessLevelService _accessLevelService;
        IHostingEnvironment _appEnvironment;


        public EmployeesController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, AccessLevelService accessLevel, IHostingEnvironment env)
        {
            _context = context;
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _accessLevelService = accessLevel;
            _appEnvironment = env;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var result = await _employeeService.FindAllAsync();
            return View(result);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                var accessLevel = await _accessLevelService.FindAllAsync();
                var viewModel = new EmployeeFormViewModel { Access_Level = accessLevel };
                return View(viewModel);
            }
            catch (Exception)
            {
                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Document,Contract_Mode,Active,Salary,Appointment,Password,Change_Password,Access_LevelId")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);

        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;

                if (ViewBag.Email == null || _context.Employee.Count(emp => emp.Id == empId) == 0)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }

                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employee.FindAsync(id);
                var accessLevel = await _accessLevelService.FindAllAsync();
                var viewModel = new EmployeeFormViewModel { Employee = employee, Access_Level = accessLevel };
                return View(viewModel);
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Document,Contract_Mode,Active,Salary,Appointment,Password,Change_Password,Access_LevelId")] Employee employee, IFormFile Document)
        {

            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);

            try
            {
                int empId = ViewBag.Id;
                if (ViewBag.Email == null)
                {
                    HttpContext.Session.SetString(SessionExpired, "true");
                    return RedirectToAction("Index", "Home", "Index");
                }


                if (ModelState.IsValid)
                {
                    try
                    {
                        ////EnviarArquivo(Document);
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
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
                return View(employee);
            }
            catch (Exception)
            {

                HttpContext.Session.SetString(SessionExpired, "true");
                return RedirectToAction("Index", "Home", "Index");
            }

        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }


        public async void EnviarArquivo(IFormFile arquivos)
        {
            // caminho completo do arquivo na localização temporária
            var caminhoArquivo = Path.GetTempFileName();

            // processa os arquivo enviados
            //percorre a lista de arquivos selecionados

                //verifica se existem arquivos 
                if (arquivos == null || arquivos.Length == 0)
                {
                    //retorna a viewdata com erro
                    ViewData["Erro"] = "Error: Arquivo(s) não selecionado(s)";
                }
                // < define a pasta onde vamos salvar os arquivos >
                string pasta = "Files";
                // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
                string nomeArquivo = "Usuario_arquivo_" + DateTime.Now.Millisecond.ToString();
                //verifica qual o tipo de arquivo : jpg, gif, png, pdf ou tmp
                if (arquivos.FileName.Contains(".jpg"))
                    nomeArquivo += ".jpg";
                else if (arquivos.FileName.Contains(".gif"))
                    nomeArquivo += ".gif";
                else if (arquivos.FileName.Contains(".png"))
                    nomeArquivo += ".png";
                else if (arquivos.FileName.Contains(".pdf"))
                    nomeArquivo += ".pdf";
                else
                    nomeArquivo += ".tmp";
                //< obtém o caminho físico da pasta wwwroot >
                string caminho_WebRoot = _appEnvironment.WebRootPath;
                // monta o caminho onde vamos salvar o arquivo : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
                string caminhoDestinoArquivo = caminho_WebRoot + pasta + "\\";
                // incluir a pasta Recebidos e o nome do arquivo enviado : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
                string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + nomeArquivo;
                //copia o arquivo para o local de destino original
                using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
                {
                    await arquivos.CopyToAsync(stream);
                }
        }
    }
}