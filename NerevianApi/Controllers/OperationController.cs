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
                    .Include(o => o.offer.request.containerType)
                    .Include(o => o.offer.request.notifications).ThenInclude(n => n.incoterm)
                    .Select(o => new
                    {
                        id = o.Id,
                        operacio_ref = o.reference,
                        estat_actual = o.status != null ? o.status.status : "En proceso",

                        // Seguridad para evitar errores si el nombre del puerto es muy corto
                        ruta_corta = (o.offer.request.originPort.name.Length >= 3 ? o.offer.request.originPort.name.Substring(0, 3) : o.offer.request.originPort.name).ToUpper() + " -> " +
                                     (o.offer.request.destinationPort.name.Length >= 3 ? o.offer.request.destinationPort.name.Substring(0, 3) : o.offer.request.destinationPort.name).ToUpper(),

                        puerto_origen = o.offer.request.originPort.name,
                        puerto_destino = o.offer.request.destinationPort.name,
                        tipo_carga = o.offer.request.cargoType != null ? o.offer.request.cargoType.type : "N/A",
                        tipo_contenedor = o.offer.request.containerType != null ? o.offer.request.containerType.Name : "Standard",

                        num_contenedor = "MSKU" + (o.Id + 8800) + "5",
                        peso = o.offer.request.pes_brut + " kg",
                        volumen = o.offer.request.volum + " m³",

                        historial = o.offer.request.notifications
                            .OrderByDescending(n => n.updateDate)
                            .Select(n => new {
                                fecha = n.updateDate.ToString("dd MMM yyyy"),
                                hora = n.updateDate.ToString("HH:mm"),
                                evento = n.incoterm != null ? n.incoterm.Name : "Actualización de sistema"
                            }).ToList(),

                        // --- CAMBIO CLAVE: Booleanos aplanados para Android ---
                        bl = true,
                        factura = true,
                        packing_list = false,
                        dua = false
                    })
                    .FirstOrDefaultAsync();

                if (operationData == null)
                    return NotFound(new { message = $"Operación {id} no encontrada" });

                return Ok(operationData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error de servidor", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.Operation
                    .Include(o => o.status)
                    .Include(o => o.offer.request.originPort)
                    .Include(o => o.offer.request.destinationPort)
                    .Select(o => new {
                        id = o.Id,
                        reference = o.reference,
                        status = o.status != null ? o.status.status : "Pendiente",
                        // Substring seguro también aquí
                        ruta = (o.offer.request.originPort.name.Length >= 3 ? o.offer.request.originPort.name.Substring(0, 3) : o.offer.request.originPort.name) + " - " +
                               (o.offer.request.destinationPort.name.Length >= 3 ? o.offer.request.destinationPort.name.Substring(0, 3) : o.offer.request.destinationPort.name)
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