using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NerevianApi.Models.Business.Offer;
using NerevianApi.Data;

namespace NerevianApi.Controllers
{
    [Route("api/offers")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly NerevianDbContext _context;
        public OfferController(NerevianDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound(new { message = $"No se encontró la oferta con ID {id}." });
            }
            else {
                return Ok(offer);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] Offer newOffer)
        {
            newOffer.finalValidationDate = DateTime.Now;
            if (newOffer.estat_oferta_id == 0)
            {
                newOffer.estat_oferta_id = 0;
            }

            _context.Offers.Add(newOffer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOffer), new { id = newOffer.id }, newOffer);

        }

        





    }

  
}