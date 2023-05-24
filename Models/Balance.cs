using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Models
{
    public class Balance
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Salary { get; set; }

        public User User { get; set; }
        public List<Expense> Expenses { get; set; }

        public Balance()
        { }

        public Balance(decimal salary)
        {
            Id = Guid.NewGuid();
            Salary = salary;
        }
    }
}
