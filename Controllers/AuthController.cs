using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Data;
using PersonalFinance.Models.ViewModels;
using PersonalFinance.Utils;

namespace PersonalFinance.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AuthController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel request)
        {
            var user = _dataContext.Users.Where(u => u.Username.Equals(request.Username)).FirstOrDefault();

            if (user is null)
                return Unauthorized("Usuário não encontrado!");

            if (PasswordUtils.VerifyPasswordHash(user, request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Ok(PasswordUtils.CreateToken(user));
            }

            return Unauthorized();
        }
    }
}
