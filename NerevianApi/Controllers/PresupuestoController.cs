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
                .Include(r => r.notifications)
                    .ThenInclude(n => n.incoterm)
                .Take(20)
                .ToListAsync();

            var listaParaAndroid = new List<Presupuesto>();

            foreach (var s in solicitudesDB)
            {
                var primeraOferta = s.Offers?.FirstOrDefault();
                var pres = new Presupuesto
                {
                    Id = "EXP-" + s.Id,

                    Origen = s.originPort?.name ?? "Puerto desconocido",
                    Destino = s.destinationPort?.name ?? "Puerto desconocido",

                    Tipo = "Transporte " + s.transportTypeId,

                    Expira = "Creado: " + s.createdAt.ToString("dd MMM"),

                    Precio = primeraOferta != null
                             ? $"{primeraOferta.budget} {primeraOferta.coin}"
                             : "0 EUR",

                    Incoterm = s.notifications
                        .OrderByDescending(n => n.updateDate)
                        .Select(n => n.incoterm != null ? n.incoterm.Name : null)
                        .FirstOrDefault() ?? "Incoterm N/A",
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
