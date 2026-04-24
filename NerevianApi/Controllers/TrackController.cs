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
                var request = await _context.StatusRequests.FindAsync(id);

                if (request == null)
                {
                    return NotFound(new { message = "No se ha encontrado la operacion :(" });
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Se ha petado",
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
