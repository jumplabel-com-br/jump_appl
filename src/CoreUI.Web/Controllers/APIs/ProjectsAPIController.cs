using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;

namespace CoreUI.Web.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        const string SessionEmail = "_Email";

        public ProjectsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectsAPI
        [HttpGet]
        public dynamic GetProject(int? clientId, int? accessLevel, int? employeeId)
        {

            var Email = HttpContext.Session.GetString(SessionEmail);
            string[] ElementoVetor = new string[1] { "Acesso inválido" };

            if (Email == null)
            {
                return ElementoVetor;
            }

            if (clientId.HasValue && accessLevel == 3)
            {
                var result = from project in _context.Project
                             join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                             join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                             where project.Client_Id == clientId
                             && projectTeam.Employee_Id == employeeId
                             //where employee.Id == employeeId && projectTeam.Employee_Id == employeeId

                             select new Project()
                             {
                                 Id = project.Id,
                                 Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                                 Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                                 Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                                 Active = project.Active == null ? 0 : project.Active,
                                 Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                                 Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                             };

                return result
               .OrderBy(x => x.Project_Name)
               .Distinct();
            }

            if (clientId.HasValue && accessLevel == 2)
            {
                var result = from project in _context.Project
                             join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                             join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                             where project.Client_Id == clientId
                             && project.Project_Manager_Id == employeeId
                             //where employee.Id == employeeId && projectTeam.Employee_Id == employeeId

                             select new Project()
                             {
                                 Id = project.Id,
                                 Project_Name = project.Project_Name == null ? "" : project.Project_Name,
                                 Client_Id = project.Client_Id == null ? 0 : project.Client_Id,
                                 Cost_Center_Id = project.Cost_Center_Id == null ? 0 : project.Cost_Center_Id,
                                 Active = project.Active == null ? 0 : project.Active,
                                 Project_Manager_Id = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id,
                                 Manager_Id = project.Manager_Id == null ? 0 : project.Manager_Id
                             };

                return result
               .OrderBy(x => x.Project_Name)
               .Distinct();
            }
            else
            {
                var result = _context.Project.OrderBy(x => x.Project_Name).Distinct();
                return result
               .OrderBy(x => x.Project_Name)
               .Distinct();
            }


        }

        // GET: api/ProjectsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/ProjectsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectsAPI
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/ProjectsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}