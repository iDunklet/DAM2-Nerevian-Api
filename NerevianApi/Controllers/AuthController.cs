using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace NerevianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
  
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
   
                return BadRequest(new { message = "El correo y la contraseña son obligatorios" });
            }


            if (model.Email == "admin@nerevian.com" && model.Password == "66666")
            {
                
                return Ok(new { message = "¡Inicio de sesión exitoso!" });
            }

            return Unauthorized(new { message = "Correo o contraseña incorrectos" });
        }
    }
}