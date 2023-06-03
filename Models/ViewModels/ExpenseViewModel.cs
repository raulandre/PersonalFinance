using PersonalFinance.Enums;

namespace PersonalFinance.Models.ViewModels
{
    public class ExpenseViewModel
    {
        public EnumExpenseType Type { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public ExpenseViewModel()
        {}

        public ExpenseViewModel(EnumExpenseType type, string description, decimal cost)
        {
            Type = type;
            Description = description;
            Cost = cost;
        }
    }
}