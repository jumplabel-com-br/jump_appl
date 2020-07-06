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
    public class PricingDetailsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        const string SessionEmail = "_Email";

        public PricingDetailsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PricingDetailsAPI
        [HttpGet]
        public dynamic GetPricingDetails(int? pricingId)
        {
            var Email = HttpContext.Session.GetString(SessionEmail);
            string[] ElementoVetor = new string[1] { "Acesso inválido" };

            if (Email == null)
            {
                return ElementoVetor;
            }

            if (pricingId.HasValue)
            {

                var result = from pricingDetails in _context.PricingDetails
                             where pricingDetails.Pricing_Id == pricingId
                             select pricingDetails;

                return result.OrderBy(x => x.SpecialtyName).Distinct();

                //result = _context.DetailsPricing.Where( x => x.Pricing_Id == pricingId);
            }

            return _context.PricingDetails;

            return _context.PricingDetails;
        }

        // GET: api/PricingDetailsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPricingDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingDetails = await _context.PricingDetails.FindAsync(id);

            if (pricingDetails == null)
            {
                return NotFound();
            }

            return Ok(pricingDetails);
        }

        // PUT: api/PricingDetailsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricingDetails([FromRoute] int id, [FromBody] PricingDetails pricingDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricingDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(pricingDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingDetailsExists(id))
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

        // POST: api/PricingDetailsAPI
        [HttpPost]
        public async Task<IActionResult> PostPricingDetails([FromBody] PricingDetails pricingDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PricingDetails.Add(pricingDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPricingDetails", new { id = pricingDetails.Id }, pricingDetails);
        }

        // DELETE: api/PricingDetailsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePricingDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingDetails = await _context.PricingDetails.FindAsync(id);
            if (pricingDetails == null)
            {
                return NotFound();
            }

            _context.PricingDetails.Remove(pricingDetails);
            await _context.SaveChangesAsync();

            return Ok(pricingDetails);
        }

        private bool PricingDetailsExists(int id)
        {
            return _context.PricingDetails.Any(e => e.Id == id);
        }
    }
}