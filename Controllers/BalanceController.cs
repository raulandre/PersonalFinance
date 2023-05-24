using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Extensions;
using PersonalFinance.Models.ViewModels;

namespace PersonalFinance.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BalanceController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            var user = await _dataContext.Users.Include(u => u.Balance).FirstOrDefaultAsync(u => u.Id.Equals(this.LoggedUserId()));
            if (user is null)
                return NotFound("Usuário não encontrado.");

            var balance = user.Balance;
            return Ok(new { balanceId = balance.Id, userId = user.Id, salary = balance.Salary });
        }

        [HttpPut]
        public async Task<IActionResult> PostBalance([FromBody] BalanceViewModel balanceViewModel)
        {
            var user = await _dataContext.Users.Include(u => u.Balance).FirstOrDefaultAsync(u => u.Id.Equals(this.LoggedUserId()));
            if(user is null)
                return NotFound("Usuário não encontrado.");

            user.Balance.Salary = balanceViewModel.Salary;
            _dataContext.Entry(user.Balance).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return Ok("Balança atualizada com sucesso!");
        }
    }
}
