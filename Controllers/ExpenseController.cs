﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.Enums;
using PersonalFinance.Extensions;
using PersonalFinance.Models;
using PersonalFinance.Models.ViewModels;
using PersonalFinance.Utils;
using System.Text.Json;

namespace PersonalFinance.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ExpenseController([FromServices] DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("expense-types")]
        public IActionResult GetExpenseTypes()
        {
            var types = Enum.GetValues(typeof(EnumExpenseType)).Cast<EnumExpenseType>().ToDictionary(t => (int)t, t => t.ToString());
            return Ok(JsonSerializer.Serialize(types));
        }

        [HttpGet("expenses")]
        public async Task<IActionResult> GetExpensesByUser()
        {
            var userId = this.LoggedUserId();
            var user = await _dataContext.Users
                .Include(u => u.Balance)
                .ThenInclude(u => u.Expenses)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if(user.Balance.Expenses.Count > 0)
            {
                var expenses = user.Balance.Expenses.Select(e => new ExpenseViewModel(e.ExpenseType, e.Description, e.Cost));
                return Ok(expenses);
            }

            return NotFound("Nenhum gasto cadastrado para o usuário.");
        }

        [HttpPut("expenses")]
        public async Task<IActionResult> UpdateExpenses([FromBody] List<ExpenseViewModel> expenses)
        {
            var userId = this.LoggedUserId();
            var user = await _dataContext.Users.Include(u => u.Balance).ThenInclude(u => u.Expenses).FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user is null)
                return NotFound("Usuário não encontrado");

            user.Balance.Expenses = expenses.Select(e => new Expense(e.Type, e.Description, e.Cost, user.BalanceId)).ToList();
            _dataContext.Entry(user.Balance).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            return Ok("Gastos atualizados com sucesso");
        }
    }
}
