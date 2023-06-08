using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Enums;
using PersonalFinance.Extensions;
using PersonalFinance.Models.ViewModels;
using System.Text.Json;

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

        // GET Balance
        /// <summary>
        /// Endpoint para buscar as informações do dashboard
        /// </summary>
        /// <returns>Todas as informações para montar o dashboard</returns>
        /// <response code="200">Dados para o dashboard</response>
        /// <response code="404">Se o usuário logado não tiver balança em aberto.</response>    
        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            var user = await _dataContext.Users.Include(u => u.Balance).ThenInclude(b => b.Expenses).FirstOrDefaultAsync(u => u.Id.Equals(this.LoggedUserId()));
            if (user is null)
                return NotFound("Usuário não encontrado.");

            var balance = user.Balance;
            return Ok(new {
                balanceId = balance.Id,
                userId = user.Id,
                salary = balance.Salary,
                expensesToal = balance.Expenses.Sum(e => e.Cost),
                expensesMonthly = balance.Expenses.Select(e => new
                {
                    type = e.ExpenseType,
                    description = e.Description,
                    percentage = Math.Round(((decimal)e.Cost / (decimal)balance.Expenses.Sum(e => e.Cost)) * 100M, 2)
                }).OrderByDescending(e => e.percentage).ToList(),
                expensesByType = balance.Expenses.GroupBy(e => e.ExpenseType).Select(e => new
                {
                    type = e.Key,
                    percentage = Math.Round(((decimal)e.Count() / (decimal)balance.Expenses.Count) * 100M, 2)
                }).OrderByDescending(e => e.percentage).ToList()
            });
        }

        // PUT Balance
        /// <summary>
        /// Endpoint utilizado para atualizar o sálario do usuário
        /// </summary>
        /// <returns>Mensagem de sucesso/erro</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="404">Se o usuário logado não tiver balança em aberto.</response>  
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
