using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NerevianApi.Models.Business.Offer;

namespace NerevianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : ControllerBase
    {
        private static List<Offer> _fakeDatabase = new List<Offer>
        {
            new Offer {
                id = 1,
                denyReason = string.Empty,
                status = new StatusOffer { id = 1, status = "pending" }
            },
            new Offer {
                id = 2,
                denyReason = string.Empty,
                status = new StatusOffer { id = 2, status = "pending" }
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Offer>> GetOffers()
        {
            return Ok(_fakeDatabase);
        }

        [HttpPost("{id}/status")]
        public IActionResult UpdateOfferStatus(int id, [FromBody] OfferStatusUpdateRequest request)
        {
            if (request.NewStatus.ToLower() == "rejected" && string.IsNullOrEmpty(request.Justification))
            {
                return BadRequest(new { message = "Debe proporcionar una justificación para rechazar la propuesta." });
            }

            var offerToUpdate = _fakeDatabase.FirstOrDefault(o => o.id == id);
            if (offerToUpdate == null)
            {
                return NotFound(new { message = $"No se encontró la oferta con ID {id}." });
            }

            if (request.NewStatus.ToLower() == "rejected")
            {
                offerToUpdate.denyReason = request.Justification;
            }

            if (offerToUpdate.status == null)
            {
                offerToUpdate.status = new StatusOffer();
            }
            offerToUpdate.status.status = request.NewStatus;

            return Ok(new
            {
                message = "Estado de la oferta actualizado correctamente.",
                updatedOffer = offerToUpdate
            });
        }
    }

    public class OfferStatusUpdateRequest
    {
        public string NewStatus { get; set; } = string.Empty;
        public string? Justification { get; set; }
    }
}