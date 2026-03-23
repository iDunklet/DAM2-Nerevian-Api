using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NerevianApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult GetGreeting() { }
        
    }
}
