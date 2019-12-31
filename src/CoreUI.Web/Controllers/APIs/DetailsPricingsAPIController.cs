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
    public class DetailsPricingsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetailsPricingsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetailsPricingsAPI
        [HttpGet]
        public IEnumerable<DetailsPricing> GetDetailsPricing(int? pricingId)
        {

            if (pricingId.HasValue)
            {

                var result = from detailsPricings in _context.DetailsPricing
                         where detailsPricings.Pricing_Id == pricingId
                         select detailsPricings;

                return result.OrderBy(x => x.SpecialtyName).Distinct();
                
                //result = _context.DetailsPricing.Where( x => x.Pricing_Id == pricingId);
            }

            return _context.DetailsPricing;
        }

        // GET: api/DetailsPricingsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsPricing([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detailsPricing = await _context.DetailsPricing.FindAsync(id);

            if (detailsPricing == null)
            {
                return NotFound();
            }

            return Ok(detailsPricing);
        }

        // PUT: api/DetailsPricingsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetailsPricing([FromRoute] int id, [FromBody] DetailsPricing detailsPricing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != detailsPricing.Id)
            {
                return BadRequest();
            }

            _context.Entry(detailsPricing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailsPricingExists(id))
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

        // POST: api/DetailsPricingsAPI
        [HttpPost]
        public async Task<IActionResult> PostDetailsPricing([FromBody] DetailsPricing detailsPricing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DetailsPricing.Add(detailsPricing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetailsPricing", new { id = detailsPricing.Id }, detailsPricing);
        }

        // DELETE: api/DetailsPricingsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetailsPricing([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detailsPricing = await _context.DetailsPricing.FindAsync(id);
            if (detailsPricing == null)
            {
                return NotFound();
            }

            _context.DetailsPricing.Remove(detailsPricing);
            await _context.SaveChangesAsync();

            return Ok(detailsPricing);
        }

        private bool DetailsPricingExists(int id)
        {
            return _context.DetailsPricing.Any(e => e.Id == id);
        }
    }
}