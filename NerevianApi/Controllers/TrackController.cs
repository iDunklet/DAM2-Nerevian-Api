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
                var trackingInfo = await _context.Operacions
                    .Where(o => o.Id == id)
                    .Include(o => o.Oferta)
                        .ThenInclude(of => of.Solicitud)
                            .ThenInclude(s => s.PortOrigen)
                    .Include(o => o.Oferta.Solicitud.PortDesti)
                    .Include(o => o.Oferta.Solicitud.TipusCarrega)
                    .Select(o => new
                    {
                        Operacio = o.CodiReferencia, 
                        Port_origen = o.Oferta.Solicitud.PortOrigen.Nom,
                        Port_desti = o.Oferta.Solicitud.PortDesti.Nom,
                        Tipus_carrega = o.Oferta.Solicitud.TipusCarrega.Tipus,
                        Data_inici = o.DataInici,
                        Estat_id = o.EstatId
                    })
                    .FirstOrDefaultAsync();

                if (trackingInfo == null)
                {
                    return NotFound(new { message = "No se ha encontrado la operacion con ID: " + id });
                }

                return Ok(trackingInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno en el servidor",
                    error = ex.Message
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
