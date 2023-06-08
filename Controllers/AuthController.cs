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
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AuthController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // POST Auth/login
        /// <summary>
        /// Endpoint para realizar login na API
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Informações sobre o usuário logado, incluindo o token e o status de onboarding.</returns>
        /// <response code="200">Retorna informações sobre o usuário</response>
        /// <response code="401">Se o usuário não for encontrado ou nome de usuário/senha estiverem incorretos.</response>    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            var user = await _dataContext.Users.Where(u => u.Username.Equals(request.Username)).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized("Usuário não encontrado!");

            if (PasswordUtils.VerifyPasswordHash(user, request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Ok(new { userId = user.Id, token = PasswordUtils.CreateToken(user), onboarding = user.Onboarding, username = user.Username, email = user.Email });
            }

            return Unauthorized("Erro ao realizar login, verifique seu nome de usuário e senha!");
        }

        // POST Auth/register
        /// <summary>
        /// Endpoint para realizar cadastro de usuário na API
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Mensagem indicando se registro ocorreu com sucesso ou não.</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Se o nome de usuário/senha estiverem em uso.</response>    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel request)
        {
            PasswordUtils.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);
            var user = new User(request.Username, request.Email.ToLower(), hash, salt);

            if (_dataContext.Users.Any(u => u.Username.Equals(user.Username) || u.Email.Equals(user.Email)))
                return BadRequest("Nome de usuário e/ou email em uso.");

            user.Balance = new Balance(0);

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return Ok("Usuário registrado com sucesso! Faça login para continuar");
        }
    }
}
