using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinance.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ControllerTesteAutenticacao : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Autenticado!");
        }
    }
}
