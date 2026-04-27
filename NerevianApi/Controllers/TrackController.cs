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


        // 2. LISTA DE OPERACIONES (GET api/tracks)
        [HttpGet]
        public async Task<IActionResult> GetAllTracks()
        {
            try
            {
                var trackingList = await _context.Operation
                    .Include(o => o.status)
                    .Include(o => o.client).ThenInclude(c => c.User)
                    .Include(o => o.offer).ThenInclude(off => off.request).ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Select(o => new
                    {
                        id = o.Id,
                        referenceCode = o.reference,
                        status = o.status != null ? o.status.status : "Desconocido",
                        clientName = o.client != null && o.client.User != null
                            ? o.client.User.Name + " " + o.client.User.Surname
                            : "N/A",
                        originPort = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        destinationPort = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
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

        // 3. ACTUALIZAR ESTADO (PUT api/tracks/{id}/estado/{nuevoEstadoId})
        [HttpPut("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> ChangeStatus(int id, int nuevoEstadoId)
        {
            var operation = await _context.Operation.FindAsync(id);
            if (operation == null) return NotFound();

            operation.StatusId = nuevoEstadoId;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Estado de operación actualizado correctamente" });
        }
    }
}