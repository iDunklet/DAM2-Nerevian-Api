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

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            try
            {
                // Buscamos en la tabla 'usuaris' mapeando los nombres reales de la BD
                var user = await _context.Usuaris
                    .Where(u => u.id == id)
                    .Select(u => new
                    {
                        id = u.id,
                        firstName = u.nom,      // En BD es 'nom'
                        lastName = u.cognoms,   // En BD es 'cognoms'
                        email = u.correu,       // En BD es 'correu'
                        phone = u.telefon,      // En BD es 'telefon'
                        roleId = u.rol_id       // En BD es 'rol_id'
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }

                return Ok(user);
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