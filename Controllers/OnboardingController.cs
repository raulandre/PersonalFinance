using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Extensions;

namespace PersonalFinance.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OnboardingController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET Onboarding/onboarding-state
        /// <summary>
        /// Busca o estado em que o usuário se encontra no processo de onboarding.
        /// </summary>
        /// <returns>Numero representando etapa do onboarding que o usuário parou.</returns>
        /// <response code="200">Etapa do onboarding</response>
        /// <response code="404">Mensagem de erro</response>
        [HttpGet("onboarding-state")]
        public async Task<IActionResult> GetOnboardingState()
        {
            var userId = this.LoggedUserId();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if(user is null)
                return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                user.Onboarding
            });
        }

        // PUT Onboarding/onboarding-state/{state}
        /// <summary>
        /// Atualiza a etapa do onboarding do usuário. Não deve ser utilizado para finalizar o onboarding!
        /// </summary>
        /// <returns>Mensagem de erro/sucesso.</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="404">Mensagem de erro</response>
        [HttpPut("update-onboarding/{state}")]
        public async Task<IActionResult> UpdateOnboardingState(byte state)
        {
            var userId = this.LoggedUserId();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user is null)
                return NotFound("Usuário não encontrado.");

            user.Onboarding = state;
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            return Ok("Status de onboarding atualizado!");
        }

        // PUT Onboarding/finish-onboarding
        /// <summary>
        /// Finaliza o onboarding do usuário, setando onboarding = null
        /// </summary>
        /// <returns>Mensagem de erro/sucesso.</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="404">Mensagem de erro</response>
        [HttpPut("finish-onboarding")]
        public async Task<IActionResult> FinishOnboardinig()
        {
            var userId = this.LoggedUserId();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user is null)
                return NotFound("Usuário não encontrado.");

            user.Onboarding = null;
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            return Ok("Onboarding finalizado!");
        }
    }
}
