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
    }
}
