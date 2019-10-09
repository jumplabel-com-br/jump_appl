using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;

namespace CoreUI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Project_teamAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Project_teamAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Project_teamAPI
        [HttpGet]
        public IEnumerable<Project_team> GetProject_team()
        {
            return _context.Project_team;
        }

        // GET: api/Project_teamAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject_team([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project_team = await _context.Project_team.FindAsync(id);

            if (project_team == null)
            {
                return NotFound();
            }

            return Ok(project_team);
        }

        // PUT: api/Project_teamAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject_team([FromRoute] int id, [FromBody] Project_team project_team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project_team.Id)
            {
                return BadRequest();
            }

            _context.Entry(project_team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Project_teamExists(id))
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

        // POST: api/Project_teamAPI
        [HttpPost]
        public async Task<IActionResult> PostProject_team([FromBody] Project_team project_team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Project_team.Add(project_team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject_team", new { id = project_team.Id }, project_team);
        }

        // DELETE: api/Project_teamAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject_team([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project_team = await _context.Project_team.FindAsync(id);
            if (project_team == null)
            {
                return NotFound();
            }

            _context.Project_team.Remove(project_team);
            await _context.SaveChangesAsync();

            return Ok(project_team);
        }

        private bool Project_teamExists(int id)
        {
            return _context.Project_team.Any(e => e.Id == id);
        }
    }
}