using Microsoft.EntityFrameworkCore;
using PersonalFinance.Models;

namespace PersonalFinance.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Balance>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Expense>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Balance> Balances { get; set; }
    }
}
