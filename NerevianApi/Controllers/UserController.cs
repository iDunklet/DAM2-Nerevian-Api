using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NerevianApi.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace NerevianApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NerevianDbContext _context;

        public UserController(NerevianDbContext context)
        {
            _context = context;
        }

        // GET: api/user/perfil/5
        [HttpGet("perfil/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            try
            {
                // Buscamos al usuario en la tabla 'usuaris'
                var usuario = await _context.Usuaris
                    .Where(u => u.id == id)
                    .Select(u => new
                    {
                        id = u.id,
                        nom = u.nom,
                        cognoms = u.cognoms,
                        correu = u.correu,
                        telefon = u.telefon,
                        rol_id = u.rol_id
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return NotFound(new { message = "No se ha encontrado el usuario" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener el perfil",
                    error = ex.Message
                });
            }
        }
    }
}