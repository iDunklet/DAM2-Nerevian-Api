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
                    .Include(o => o.status) // Tabla estats_operacions (estat_id)
                    .Include(o => o.offer).ThenInclude(off => off.request).ThenInclude(r => r.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Include(o => o.offer.request.cargoType)
                    // Incluimos notificaciones e incoterm para el historial
                    .Include(o => o.offer.request.notifications).ThenInclude(n => n.incoterm)
                    .Select(o => new
                    {
                        // --- Datos de la tabla OPERACIONS ---
                        id = o.Id,
                        // 'reference' mapea a 'codi_referencia' en la tabla operacions
                        operacio = o.reference,
                        // 'StatusId' mapea a 'estat_id' en la tabla operacions
                        estado_id = o.StatusId,
                        // Texto del estado desde 'estats_operacions'
                        estat_actual = o.status != null ? o.status.status : "Desconocido",
                        // Fecha de inicio de la operación
                        data_inici = o.InitialDate.HasValue ? o.InitialDate.Value.ToString("yyyy-MM-dd") : "N/A",

                        // --- Datos de la tabla SOLICITUD (vía Offer) ---
                        port_origen = o.offer.request.originPort != null ? o.offer.request.originPort.name : "N/A",
                        port_desti = o.offer.request.destinationPort != null ? o.offer.request.destinationPort.name : "N/A",
                        tipus_carrega = o.offer.request.cargoType != null ? o.offer.request.cargoType.type : "N/A",
                        pes_brut = o.offer.request.pes_brut,

                        incoterm = "FOB", // Valor por defecto o sacado de la solicitud si fuera necesario

                        cliente_nombre = o.client != null && o.client.User != null
                            ? o.client.User.Name + " " + o.client.User.Surname
                            : "Cliente Desconocido",

                        // --- Historial (Notificaciones de la solicitud) ---
                        historial = o.offer.request.notifications
                            .OrderByDescending(n => n.updateDate)
                            .Select(n => new {
                                // 'updateDate' mapea a 'date_update' según tu SQL
                                fecha = n.updateDate.ToString("yyyy-MM-dd"),
                                // 'incoterm.name' mapea a la tabla Incoterm (FK incoterm_id)
                                evento = n.incoterm != null ? n.incoterm.Name : "Estado actualizado"
                            }).ToList(),

                        // Documentación (Mockup/Fijo)
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
                // Este catch capturará errores de nombres de columna si los modelos no están sincronizados
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
                        // Usamos la referencia de la operación
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

        [HttpPut("{id}/estado/{nuevoEstadoId}")]
        public async Task<IActionResult> ChangeStatus(int id, int nuevoEstadoId)
        {
            try
            {
                var operation = await _context.Operation.FindAsync(id);
                if (operation == null) return NotFound(new { message = "Operación no encontrada" });

                // Actualiza el estat_id de la tabla operacions
                operation.StatusId = nuevoEstadoId;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Estado actualizado correctamente en la operación" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar", error = ex.Message });
            }
        }
    }
}