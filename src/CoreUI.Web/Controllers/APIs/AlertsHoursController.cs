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
    public class AlertsHoursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";

        public AlertsHoursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AlertsHours
        [HttpGet]
        public dynamic GetHour()
        {
            var Email = HttpContext.Session.GetString(SessionEmail);
            var Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            var Name = HttpContext.Session.GetString(SessionName);
            var AcessLevel = HttpContext.Session.GetInt32(SessionAcessLevel);
            var TotalMessagesBells = HttpContext.Session.GetInt32(SessionTotalBells);

            string[] ElementoVetor = new string[1] { "Acesso inválido" };


            if (Email == null)
            {
                return ElementoVetor;
            }

            if (AcessLevel == 3)
            {
                return _context.Hour
                    .Where(x => x.Approval == 2 || x.Approval == 3 || x.Approval == 4 && x.Employee_Id == Id);
            }

            return _context.Hour
                .Where(x => x.Approval == 1)
                .OrderBy(x => x.Consultant)
                .GroupBy(x => x.Consultant)
                .Select(x => x.First())
                .ToList();
        }

        // GET: api/AlertsHours/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHour([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hour = await _context.Hour.FindAsync(id);

            if (hour == null)
            {
                return NotFound();
            }

            return Ok(hour);
        }

        // PUT: api/AlertsHours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHour([FromRoute] int id, [FromBody] Hour hour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hour.Id)
            {
                return BadRequest();
            }

            _context.Entry(hour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HourExists(id))
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

        // POST: api/AlertsHours
        [HttpPost]
        public async Task<IActionResult> PostHour([FromBody] Hour hour)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Hour.Add(hour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHour", new { id = hour.Id }, hour);
        }

        // DELETE: api/AlertsHours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHour([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hour = await _context.Hour.FindAsync(id);
            if (hour == null)
            {
                return NotFound();
            }

            _context.Hour.Remove(hour);
            await _context.SaveChangesAsync();

            return Ok(hour);
        }

        private bool HourExists(int id)
        {
            return _context.Hour.Any(e => e.Id == id);
        }

    }
}