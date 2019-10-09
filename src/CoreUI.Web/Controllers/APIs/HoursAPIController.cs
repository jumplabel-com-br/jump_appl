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
    public class HoursAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HoursAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/HoursAPI
        [HttpGet]
        public IEnumerable<Hour> GetHour()
        {
            return _context.Hour;
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

        // POST: api/HoursAPI
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