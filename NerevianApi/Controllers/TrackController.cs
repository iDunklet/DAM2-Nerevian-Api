using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerevianApi.Data;
using Microsoft.EntityFrameworkCore;

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
                var trackingData = await _context.Operations
                    .Where(o => o.Id == id)
                    .Include(o => o.offer) // minúscula, como en el modelo
                        .ThenInclude(off => off.request)
                            .ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Include(o => o.offer.request.cargoType)
                    .Include(o => o.status) // minúscula, como en el modelo
                    .Select(o => new
                    {
                        port_origen = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        port_desti = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
                        tipus_carrega = o.offer.request.cargoType != null ? o.offer.request.cargoType.type : "N/A",
                        operacio = o.reference, // minúscula
                        estat_actual = o.status != null ? o.status.status : "Desconocido", // minúscula
                        data_inici = o.InitialDate,
                        observaciones = o.observations // minúscula
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

        // Endpoint para cambiar estado
        [HttpPut("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> ChangeStatus(int id, int nuevoEstadoId)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { message = "No se ha encontrado la operacion :(" });
            }
            else
            {
                request.estat_solicitud_id = nuevoEstadoId;
                await _context.SaveChangesAsync();
                return Ok(request.estat_solicitud_id);
            }
        }

        // Endpoint para LA LISTA ENTERA (Para el RecyclerView de Android)
        [HttpGet]
        public async Task<IActionResult> GetAllTracks()
        {
            try
            {
                var trackingList = await _context.Operations
                    .Include(o => o.status)
                    .Include(o => o.client)
                        .ThenInclude(c => c.User) // ¡Ahora funcionará gracias al Client.cs actualizado!
                    .Include(o => o.offer)
                        .ThenInclude(off => off.request)
                            .ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Select(o => new
                    {
                        id = o.Id,
                        referenceCode = o.reference,
                        status = o.status != null ? o.status.status : "Desconocido",
                        clientName = o.client != null && o.client.User != null ? o.client.User.Name + " " + o.client.User.Surname : "Cliente Desconocido",
                        originPort = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        destinationPort = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
                        eta = o.FinalDate
                    })
                    .ToListAsync();

                return Ok(trackingList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la lista de tracks", error = ex.Message, inner = ex.InnerException?.Message });
            }
        }
    }
}