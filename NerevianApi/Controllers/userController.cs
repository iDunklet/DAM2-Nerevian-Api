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
                // Buscamos en la tabla 'Usuaris' mapeando los nombres de BD a nombres JSON estándar
                var user = await _context.Usuaris
                    .Where(u => u.id == id)
                    .Select(u => new
                    {
                        id = u.id,
                        firstName = u.nom,      // Mapea 'nom' a 'firstName' para el Front
                        lastName = u.cognoms,   // Mapea 'cognoms' a 'lastName' para el Front
                        email = u.correu,
                        phone = u.telefon,
                        roleId = u.rol_id
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado en la base de datos." });
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