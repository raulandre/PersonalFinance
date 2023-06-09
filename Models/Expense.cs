﻿using PersonalFinance.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models
{
    public class Expense
    {
        public Guid Id { get; set; }
        public EnumExpenseType ExpenseType { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        [ForeignKey("BalanceId")]
        public Guid BalanceId { get; set; }
        public Balance Balance { get; set; }

        public Expense()
        {
            
        }

        public Expense(EnumExpenseType type, string description, decimal cost, Guid balanceId)
        {
            ExpenseType = type;
            Description = description;
            Cost = cost;
            BalanceId = balanceId;
        }
    }
}
