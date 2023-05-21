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
    [Authorize]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public BalanceController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBalance(Guid userId)
        {
            var user = await _dataContext.Users.Include(u => u.Balance).FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if (user is null)
                return NotFound("Usuário não encontrado.");

            var balance = user.Balance;
            return Ok(new { balanceId = balance.Id, userId = user.Id, salary = balance.Salary });
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> PostBalance(Guid userId, [FromBody] BalanceViewModel balanceViewModel)
        {
            var user = await _dataContext.Users.Include(u => u.Balance).FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if(user is null)
                return NotFound("Usuário não encontrado.");

            user.Balance.Salary = balanceViewModel.Salary;
            _dataContext.Entry(user.Balance).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return Ok("Balança atualizada com sucesso!");
        }
    }
}
