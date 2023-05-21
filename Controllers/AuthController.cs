using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Models;
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
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            var user = await _dataContext.Users.Where(u => u.Username.Equals(request.Username)).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized("Usuário não encontrado!");

            if (PasswordUtils.VerifyPasswordHash(user, request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Ok(PasswordUtils.CreateToken(user));
            }

            return Unauthorized("Erro ao realizar login, verifique seu nome de usuário e senha!");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel request)
        {
            PasswordUtils.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);
            var user = new User(request.Username, request.Email.ToLower(), hash, salt);

            if (_dataContext.Users.Any(u => u.Username.Equals(user.Username) || u.Email.Equals(user.Email)))
                return BadRequest("Nome de usuário e/ou email em uso.");

            var userCreated = await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return Ok("Usuário registrado com sucesso! Faça login para continuar");
        }
    }
}
