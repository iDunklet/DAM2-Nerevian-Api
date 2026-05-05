using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using NerevianApi.Models.Documents;
using NerevianApi.Models.Business.Offer;
using System;
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
        public async Task<IActionResult> Post([FromBody] CreatePresupuestoRequest request)
        {
            if (request.SolicitudId <= 0)
            {
                return BadRequest(new { message = "La solicitud es obligatoria." });
            }

            if (request.ClientId <= 0)
            {
                return BadRequest(new { message = "El cliente es obligatorio." });
            }

            if (request.Presupuesto <= 0)
            {
                return BadRequest(new { message = "El presupuesto debe ser mayor que 0." });
            }

            var solicitudExiste = await _context.Requests
                .AnyAsync(r => r.Id == request.SolicitudId);

            if (!solicitudExiste)
            {
                return NotFound(new { message = $"No existe la solicitud {request.SolicitudId}." });
            }

            var clienteExiste = await _context.Clients
                .AnyAsync(c => c.Id == request.ClientId);

            if (!clienteExiste)
            {
                return NotFound(new { message = $"No existe el cliente {request.ClientId}." });
            }

            var estadoPendiente = await FindPendingOfferStatus();
            if (estadoPendiente == null)
            {
                return BadRequest(new { message = "No existe un estado de oferta pendiente/enviada." });
            }

            var fechaCreacion = DateTime.Now;
            var oferta = new Offer
            {
                creationDate = fechaCreacion,
                initialValidationDate = request.FechaValidezInicial ?? fechaCreacion,
                finalValidationDate = request.FechaValidezFinal ?? fechaCreacion.AddDays(30),
                coin = string.IsNullOrWhiteSpace(request.Moneda) ? "EUR" : request.Moneda.Trim().ToUpperInvariant(),
                budget = request.Presupuesto,
                comments = request.Comentarios,
                denyReason = null,
                estat_oferta_id = estadoPendiente.id,
                client_id = request.ClientId,
                solicitud_id = request.SolicitudId,
                isCounterOffer = request.EsContraoferta
            };

            _context.Offers.Add(oferta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new
            {
                id = "OFE-" + oferta.id,
                message = "Presupuesto creado correctamente",
                estado = NormalizeEstado(estadoPendiente.status)
            });
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

        private async Task<StatusOffer?> FindPendingOfferStatus()
        {
            var statuses = await _context.StatusOffers.ToListAsync();

            return statuses.FirstOrDefault(s => s.status.Equals("enviada", StringComparison.OrdinalIgnoreCase))
                ?? statuses.FirstOrDefault(s => NormalizeEstado(s.status) == "Pendiente");
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

    public class CreatePresupuestoRequest
    {
        public int SolicitudId { get; set; }
        public int ClientId { get; set; }
        public double Presupuesto { get; set; }
        public string? Moneda { get; set; }
        public DateTime? FechaValidezInicial { get; set; }
        public DateTime? FechaValidezFinal { get; set; }
        public string? Comentarios { get; set; }
        public bool EsContraoferta { get; set; }
    }
}
