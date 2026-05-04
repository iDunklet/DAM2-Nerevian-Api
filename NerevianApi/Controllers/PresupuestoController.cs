using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using NerevianApi.Models.Documents;
using NerevianApi.Models.Business.Offer;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NerevianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestosController : ControllerBase
    {
        private readonly NerevianDbContext _context;

        public PresupuestosController(NerevianDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Presupuesto>>> Get()
        {

            var solicitudesDB = await _context.Requests
                .AsNoTracking()
                .Include(r => r.originPort)
                .Include(r => r.destinationPort)
                .Include(r => r.transportType)
                .Include(r => r.cargoType)
                .Include(r => r.containerType)
                .Include(r => r.incoterm)
                .Include(r => r.status)
                .Include(r => r.Offers)
                    .ThenInclude(o => o.status)
                .Include(r => r.Offers)
                    .ThenInclude(o => o.client)
                        .ThenInclude(c => c.User)
                .Include(r => r.notifications)
                    .ThenInclude(n => n.incoterm)
                .OrderByDescending(r => r.createdAt)
                .Take(20)
                .ToListAsync();

            var listaParaAndroid = new List<Presupuesto>();

            foreach (var s in solicitudesDB)
            {
                var oferta = s.Offers
                    .OrderByDescending(o => o.finalValidationDate)
                    .ThenByDescending(o => o.initialValidationDate)
                    .FirstOrDefault();
                if (oferta == null)
                {
                    continue;
                }

                var pres = new Presupuesto
                {
                    Id = "OFE-" + oferta.id,

                    Origen = s.originPort?.name ?? "Puerto desconocido",
                    Destino = s.destinationPort?.name ?? "Puerto desconocido",

                    Tipo = s.transportType?.type ?? "Transporte " + s.transportTypeId,

                    Expira = oferta.finalValidationDate.ToString("dd MMM yyyy"),

                    Precio = oferta.budget != null
                             ? $"{oferta.budget:0.##} {FormatCurrency(oferta.coin)}"
                             : "0 €",


                    Incoterm = s.incoterm?.Name ?? s.notifications
                        .OrderByDescending(n => n.updateDate)
                        .Select(n => n.incoterm != null ? n.incoterm.Name : null)
                        .FirstOrDefault() ?? "Incoterm N/A",
                    Detalle = BuildDetalle(s, oferta),
                    Estado = NormalizeEstado(oferta.status?.status ?? s.status?.status)
                };

                listaParaAndroid.Add(pres);
            }

            return Ok(listaParaAndroid);
        }

        [HttpPost]
        public async Task<ActionResult<Presupuesto>> Post(Presupuesto nuevo)
        {
          
            return Ok(nuevo);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(string id, [FromBody] PresupuestoEstadoUpdateRequest request)
        {
            var offerId = ParseOfferId(id);
            if (offerId == null)
            {
                return BadRequest(new { message = "El id del presupuesto no es valido." });
            }

            var offer = await _context.Offers
                .Include(o => o.status)
                .FirstOrDefaultAsync(o => o.id == offerId.Value);

            if (offer == null)
            {
                return NotFound(new { message = $"No se encontro la oferta {id}." });
            }

            var status = await FindOfferStatus(request.Estado);
            if (status == null)
            {
                return BadRequest(new { message = $"No existe un estado de oferta compatible con '{request.Estado}'." });
            }

            offer.estat_oferta_id = status.id;
            offer.denyReason = NormalizeEstado(request.Estado) == "Rechazado" ? request.Motivo : null;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Presupuesto actualizado correctamente",
                estado = NormalizeEstado(status.status)
            });
        }

        private static string BuildDetalle(NerevianApi.Models.Business.Request.Request solicitud, Offer oferta)
        {
            var partes = new List<string>
            {
                "Carga: " + (solicitud.cargoType?.type ?? "N/A"),
                "Contenedor: " + (solicitud.containerType?.Name ?? "N/A"),
                "Peso: " + (solicitud.pes_brut.HasValue ? $"{solicitud.pes_brut:0.##} kg" : "N/A"),
                "Volumen: " + (solicitud.volum.HasValue ? $"{solicitud.volum:0.##} m3" : "N/A")
            };

            if (!string.IsNullOrWhiteSpace(oferta.client?.User?.Name))
            {
                partes.Add("Cliente: " + oferta.client.User.Name + " " + oferta.client.User.Surname);
            }

            if (!string.IsNullOrWhiteSpace(oferta.comments))
            {
                partes.Add("Oferta: " + oferta.comments);
            }

            partes.Add("Solicitud: " + (solicitud.comments ?? "Sin detalles"));

            return string.Join("\n", partes);
        }



        private static int? ParseOfferId(string id)
        {
            if (int.TryParse(id, out var numericId))
            {
                return numericId;
            }

            var normalized = id.Replace("OFE-", "").Replace("EXP-", "");
            return int.TryParse(normalized, out numericId) ? numericId : null;
        }

        private async Task<StatusOffer?> FindOfferStatus(string estado)
        {
            var normalized = NormalizeEstado(estado);
            var statuses = await _context.StatusOffers.ToListAsync();

            return statuses.FirstOrDefault(s => NormalizeEstado(s.status) == normalized)
                ?? statuses.FirstOrDefault(s => s.status.Contains(normalized, System.StringComparison.OrdinalIgnoreCase));
        }

        private static string FormatCurrency(string? currency)
        {
            return currency?.Trim().ToUpperInvariant() switch
            {
                "EUR" => "€",
                "USD" => "$",
                "GBP" => "£",
                _ => string.IsNullOrWhiteSpace(currency) ? "€" : currency.Trim()
            };
        }

        private static string NormalizeEstado(string? estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
            {
                return "Pendiente";
            }

            var lower = estado.Trim().ToLowerInvariant();

            if (lower.Contains("acept") || lower.Contains("accept") || lower.Contains("aprob"))
            {
                return "Aceptado";
            }

            if (lower.Contains("rechaz") || lower.Contains("rebut") || lower.Contains("deneg") || lower.Contains("expir") || lower.Contains("deny") || lower.Contains("reject"))
            {
                return "Rechazado";
            }

            return "Pendiente";
        }
    }

    public class PresupuestoEstadoUpdateRequest
    {
        public string Estado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
    }
}
