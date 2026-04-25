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
                var trackingData = await _context.Operation // Corregido a plural
                    .Where(o => o.Id == id)
                    .Include(o => o.client).ThenInclude(c => c.User)
                    .Include(o => o.offer).ThenInclude(off => off.request).ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Include(o => o.offer.request.cargoType)
                    .Include(o => o.status)
                    .Select(o => new
                    {
                        id = o.Id,
                        operacio = o.reference,
                        estat_actual = o.status != null ? o.status.status : "Desconocido",
                        port_origen = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        port_desti = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
                        tipus_carrega = o.offer.request.cargoType != null ? o.offer.request.cargoType.type : "N/A",

                        // SOLUCIÓN AL ERROR ToString:
                        data_inici = o.InitialDate.HasValue ? o.InitialDate.Value.ToString("yyyy-MM-dd") : "N/A",

                        incoterm = "FOB",
                        cliente_nombre = o.client != null && o.client.User != null ? o.client.User.Name + " " + o.client.User.Surname : "Cliente Desconocido",
                        doc_bl = true,
                        doc_factura = true,
                        doc_packing = false,
                        doc_dua = false
                    })
                    .FirstOrDefaultAsync();

                if (trackingData == null) return NotFound(new { message = $"No se encontró la operación {id}" });

                return Ok(trackingData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener detalle", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTracks()
        {
            try
            {
                var trackingList = await _context.Operation // Corregido a plural
                    .Include(o => o.status)
                    .Include(o => o.client).ThenInclude(c => c.User)
                    .Include(o => o.offer).ThenInclude(off => off.request).ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Select(o => new
                    {
                        id = o.Id,
                        referenceCode = o.reference,
                        status = o.status != null ? o.status.status : "Desconocido",
                        clientName = o.client != null && o.client.User != null ? o.client.User.Name + " " + o.client.User.Surname : "Cliente Desconocido",
                        originPort = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        destinationPort = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",

                        // SOLUCIÓN AL ERROR ToString en el campo ETA:
                        eta = o.FinalDate.HasValue ? o.FinalDate.Value.ToString("yyyy-MM-dd") : "Pendiente"
                    })
                    .ToListAsync();

                return Ok(trackingList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener lista", error = ex.Message });
            }
        }

        [HttpPut("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> ChangeStatus(int id, int nuevoEstadoId)
        {
            var operation = await _context.Operation.FindAsync(id); // Corregido a plural
            if (operation == null) return NotFound();

            operation.StatusId = nuevoEstadoId;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Estado actualizado" });
        }
    }
}