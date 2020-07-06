using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using CoreUI.Web.Models.List.API;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;

namespace CoreUI.Web.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class HoursAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HourService _hourService;
        const string SessionExpired = "false";
        const string SessionEmail = "_Email";

        public HoursAPIController(ApplicationDbContext context, HourService hour)
        {
            _context = context;
            _hourService = hour;
        }

        // GET: api/HoursAPI
        [HttpGet]
        public dynamic GetHour(string email, string senha, DateTime? dtInicio, DateTime? dtFim, string status, string defaultVerification)
        {

            return _hourService.APIHours(email, senha, dtInicio, dtFim, status, defaultVerification);
        }
        
        // POST: api/HoursAPI
        [HttpPost]
        public dynamic PostHour([FromForm] string email, [FromForm] string senha, [FromForm] DateTime? dtInicio, [FromForm] DateTime? dtFim, [FromForm] string status, [FromForm] string defaultVerification)
        {
            return GetHour(email,senha, dtInicio, dtFim, status, defaultVerification);
        }

        // GET: api/HoursAPI/5
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

        // PUT: api/HoursAPI/5
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

        // DELETE: api/HoursAPI/5
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