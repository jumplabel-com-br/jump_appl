﻿using System;
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
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        Files Files;

        // GET: Project_team
        public async Task<IActionResult> Index()
        {

            return View(await _projectTeamService.FindAllAsync());
            //return View(await _context.Project_team.ToListAsync());
            //return View(projectTeam);
        }

        // GET: Project_team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_team = await _context.Project_team.FindAsync(id);
            var employee = await _employeeService.FindAllAsync();
            var project = await _projectService.FindAllAsync();
            var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Project_team = project_team };

            if (project_team == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Project_team/Create
        public async Task<IActionResult> Create()
        {
            var employee = await _employeeService.FindAllAsync();
            var project = await _projectService.FindAllAsync();
            var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee };

            return View(viewModel);
        }

        // POST: Project_team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Project_Id,Employee_Id,Start_Date,End_Date")] Project_team project_team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project_team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project_team);
        }

        // GET: Project_team/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_team = await _context.Project_team.FindAsync(id);
            var employee = await _employeeService.FindAllAsync();
            var project = await _projectService.FindAllAsync();
            var viewModel = new ProjectTeamFormViewModel { Project = project, Employee = employee, Project_team = project_team };

            
            if (project_team == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Project_team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Project_Id,Employee_Id,Start_Date,End_Date")] Project_team project_team)
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
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project_team);
        }

        // GET: Project_team/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Project_team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project_team = await _context.Project_team.FindAsync(id);
            _context.Project_team.Remove(project_team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Project_teamExists(int id)
        {
            return _context.Project_team.Any(e => e.Id == id);
        }
    }
}
