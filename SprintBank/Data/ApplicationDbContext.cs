using Microsoft.EntityFrameworkCore;
using SprintBank.Models;

namespace SprintBank.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts  { get; set; }   
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(at => at.AccountType)
                  .HasConversion<string>();

            modelBuilder.Entity<Transaction>()
                .Property(ts=> ts.TransactionStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Transaction>()
                .Property(tt => tt.TransactionType)
                .HasConversion<string>();
        }
    }
}
