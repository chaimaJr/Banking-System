using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Tables
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Keeps Identity tables ?

            // Decimal precision for money fields
            builder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");
            
            builder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
