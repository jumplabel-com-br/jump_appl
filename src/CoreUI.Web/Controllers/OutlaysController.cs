﻿using System;
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
        IHostingEnvironment _appEnvironment;

        public OutlaysController(ApplicationDbContext context, EmployeeService employeeService, ProjectService projectService, HourService hourService, AccessLevelService accessLevelService, ClientService clientService ,OutlaysService outlaysService, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _employeeService = employeeService;
            _projectService = projectService;
            _hourService = hourService;
            _accessLevelService = accessLevelService;
            _clienteService = clientService;
            _outlaysService = outlaysService;
            _appEnvironment = appEnvironment;

        }

        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";
        public string storage = "\\Outlays\\";

        // GET: Outlays
        public async Task<IActionResult> Index()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var ViewModel = await _outlaysService.FindPerEmployee(ViewBag.Id);

            return View(ViewModel);
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

            var outlays = await _context.Outlays
                .FirstOrDefaultAsync(m => m.Id == id);
            //var employees = _employeeService.FindAllAsync();
            var projects = await _projectService.FindPerEmployeeAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clients = await _clienteService.FindAllAsync();

            var ViewModel = new OutlaysFormViewModel { Clients = clients, Projects = projects, Outlays = outlays };

            if (outlays == null)
            {
                return NotFound();
            }
            return View(ViewModel);
        }

        // GET: Outlays/Create
        public async Task<IActionResult> Create()
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            //var employees = _employeeService.FindAllAsync();
            var projects = await _projectService.FindPerEmployeeAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clients = await _clienteService.FindAllAsync();

            var ViewModel = new OutlaysFormViewModel { Clients = clients, Projects = projects };
            return View(ViewModel);
        }

        // POST: Outlays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Employee_Id,Project_Id,Client_Id,Date,NoteNumber,NoteValue,Description,File,Status")] Outlays outlays, IFormFile Document)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            if (ModelState.IsValid)
            {
                EnviarArquivo(Document, ViewBag.Id ,storage);
                _context.Add(outlays);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(outlays);
        }

        // GET: Outlays/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var outlays = await _context.Outlays.FindAsync(id);
            //var employees = _employeeService.FindAllAsync();
            var projects = await _projectService.FindPerEmployeeAsync(ViewBag.Id, ViewBag.AcessLevel);
            var clients = await _clienteService.FindAllAsync();

            var ViewModel = new OutlaysFormViewModel { Clients = clients, Projects = projects, Outlays = outlays };
            
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Employee_Id,Project_Id,Client_Id,Date,NoteNumber,NoteValue,Description,File,Status")] Outlays outlays)
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(outlays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OutlaysExists(outlays.Id))
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
            return View(outlays);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            var outlays = await _context.Outlays.FindAsync(id);
            _context.Outlays.Remove(outlays);
            await _context.SaveChangesAsync();
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

        private bool OutlaysExists(int id)
        {
            return _context.Outlays.Any(e => e.Id == id);
        }

        public async void EnviarArquivo(IFormFile Document, int id, string storage)
        {

            // < define a pasta onde vamos salvar os arquivos >
            string pasta = "Files";
            // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
            //string nomeArquivo = DateTime.Now.ToString().Replace('/','-').Replace(':', '&').Replace(" ", "") + "_" + id + "_" + Document.FileName;
            string nomeArquivo;
            if (Document.FileName != "" && Document.FileName != null)
            {
                nomeArquivo = Document.FileName;
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