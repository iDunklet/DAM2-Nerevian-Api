using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerevianApi.Data;
using NerevianApi.Models.Business.Request;

namespace NerevianApi.Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly NerevianDbContext _context;
        public TrackController(NerevianDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrack(int id)
        {
            try
            {
                // 1. Empezamos desde Operacions
                var trackingData = await _context.Operations
                    .Where(o => o.Id == id)
                    .Include(o => o.Offer)
                        .ThenInclude(off => off.request)
                            .ThenInclude(r => r.originPort)
                    .Include(o => o.Offer.request.destinationPort)
                    .Include(o => o.Offer.request.cargoType)
                    .Include(o => o.Status) // Incluimos el estado de la operación
                    .Select(o => new
                    {
                        // 2. Mapeo exacto para tu compañero
                        port_origen = o.Offer.request.originPort != null ? o.Offer.request.originPort.name : "N/A",
                        port_desti = o.Offer.request.destinationPort != null ? o.Offer.request.destinationPort.name : "N/A",
                        tipus_carrega = o.Offer.request.cargoType != null ? o.Offer.request.cargoType.type : "N/A",
                        operacio = o.Reference, // El código (ej: 'OP-123')

                        // 3. Campos extra útiles
                        estat_actual = o.Status != null ? o.Status.status : "Desconocido",
                        data_inici = o.InitialDate,
                        observaciones = o.Observations
                    })
                    .FirstOrDefaultAsync();

                if (trackingData == null)
                {
                    return NotFound(new { message = $"No se encontró la operación {id}" });
                }

                return Ok(trackingData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error en el servidor al obtener tracking",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> ChangeStatus(int id, int nuevoEstadoId) { 
        
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { message = "No se ha encontrado la operacion :(" });
            }
            else { 
                request.estat_solicitud_id = nuevoEstadoId;
                await _context.SaveChangesAsync();
                return Ok(request.estat_solicitud_id);
            }


        }
        
    }
}
