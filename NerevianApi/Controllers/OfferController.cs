using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NerevianApi.Models.Business.Offer;

namespace NerevianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class OfferController : ControllerBase
    {
 
        [HttpGet]
        public ActionResult<IEnumerable<Offer>> GetOffers()
        {
   
            List<Offer> offers = new List<Offer>();

            return Ok(offers); 
        }

        
        [HttpPost("{id}/status")]
        public IActionResult UpdateOfferStatus(int id, [FromBody] OfferStatusUpdateRequest request)
        {
            
            if (request.NewStatus == StatusOffer.Rejected && string.IsNullOrEmpty(request.Justification))
            {
                
                return BadRequest(new { message = "Debe proporcionar una justificación para rechazar la propuesta." });
            }

            return Ok(new { message = "Estado de la oferta actualizado correctamente." });
        }
    }

    public class OfferStatusUpdateRequest
    {
        public StatusOffer NewStatus { get; set; } 
        public string Justification { get; set; }  
    }
}
