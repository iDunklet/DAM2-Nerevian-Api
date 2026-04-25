using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerevianApi.Data;
using Microsoft.EntityFrameworkCore;

namespace NerevianApi.Controllers
{
    [Route("api/operation")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly NerevianDbContext _context;

        public OperationController(NerevianDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperationDetails(int id)
        {
            try
            {
                var operationData = await _context.Operation
                    .Where(o => o.Id == id)
                    .Include(o => o.client).ThenInclude(c => c.User)
                    .Include(o => o.status)
                    .Include(o => o.offer).ThenInclude(off => off.request).ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Include(o => o.offer.request.cargoType)
                    .Include(o => o.offer.request.notifications).ThenInclude(n => n.incotermType)
                    .Select(o => new
                    {
                        id = o.Id,
                        operacio = o.reference, // De tabla operacions
                        estat_actual = o.status != null ? o.status.status : "Desconocido",

                        port_origen = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        port_desti = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
                        tipus_carrega = o.offer.request.cargoType != null ? o.offer.request.cargoType.type : "N/A",

                        data_inici = o.InitialDate.HasValue ? o.InitialDate.Value.ToString("yyyy-MM-dd") : "N/A",
                        incoterm = "FOB",
                        cliente_nombre = o.client != null && o.client.User != null
                            ? o.client.User.Name + " " + o.client.User.Surname
                            : "Cliente Desconocido",

                        pes_brut = o.offer.request.pes_brut,
                        estado_id = o.StatusId, // De tabla operacions

                        historial = o.offer.request.notifications
                            .OrderByDescending(n => n.updateDate)
                            .Select(n => new {
                                fecha = n.updateDate.ToString("yyyy-MM-dd"),
                                evento = n.incotermType != null ? n.incotermType.Name : "Estado actualizado"
                            }).ToList(),

                        doc_bl = true,
                        doc_factura = true,
                        doc_packing = false,
                        doc_dua = false
                    })
                    .FirstOrDefaultAsync();

                if (operationData == null)
                    return NotFound(new { message = $"No se encontró la operación con ID {id}" });

                return Ok(operationData);
            }
            catch (Exception ex)
            {
                // El error "Invalid object name" debería desaparecer tras el cambio en el DbContext
                return StatusCode(500, new { message = "Error al obtener detalle", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.Operation
                    .Include(o => o.status)
                    .Select(o => new {
                        id = o.Id,
                        referenceCode = o.reference,
                        status = o.status != null ? o.status.status : "Desconocido"
                    }).ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}