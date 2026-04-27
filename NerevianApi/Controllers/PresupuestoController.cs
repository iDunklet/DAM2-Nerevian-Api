using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using NerevianApi.Models.Documents;
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
                .Include(r => r.originPort)
                .Include(r => r.destinationPort)
                .Include(r => r.Offers)
                .Take(20)
                .ToListAsync();

            var listaParaAndroid = new List<Presupuesto>();

            foreach (var s in solicitudesDB)
            {
                var primeraOferta = s.Offers?.FirstOrDefault();
                var pres = new Presupuesto
                {
                    Id = "EXP-" + s.Id,

                    Origen = s.originPort?.Name ?? "Puerto desconocido",
                    Destino = s.destinationPort?.Name ?? "Puerto desconocido",

                    Tipo = "Transporte " + (s.TipusTransportId?.ToString() ?? ""),

                    Expira = "Creado: " + s.createdAt.ToString("dd MMM"),

                    Precio = primeraOferta != null
                             ? $"{primeraOferta.budget} {primeraOferta.coin}"
                             : "0 EUR",

                    Incoterm = "Incoterm " + (s.IncotermId?.ToString() ?? "N/A"),
                    Detalle = "Comentarios: " + (s.comments ?? "Sin detalles"),
                    Estado = "Estado " + s.estat_solicitud_id
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
    }
}