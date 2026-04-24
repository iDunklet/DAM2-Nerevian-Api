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
                // USAMOS '_context.Users' porque así está definido en NerevianDbContext
                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(u => new
                    {
                        id = u.Id,
                        firstName = u.Name,      // Mapea la propiedad 'Name' del modelo
                        lastName = u.Surname,    // Mapea la propiedad 'Surname' del modelo
                        email = u.email,         // Propiedad 'email'
                        phone = u.PhoneNumber,   // Propiedad 'PhoneNumber'
                        roleId = u.Roleid        // Propiedad 'Roleid'
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