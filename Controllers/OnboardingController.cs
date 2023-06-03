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
