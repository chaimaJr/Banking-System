using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AccountExists(int accountId, string userId)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Id == accountId && a.UserId == userId);
        }

        public async Task<IEnumerable<Account>> GetAccountList()
        {
            return await _context.Accounts
                .Where(a => a.IsActive)
                .Include(a => a.Transactions.Take(5))
                .ToListAsync();
        }

        public async Task<Account?> GetAccountById(int id)
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Account>?> GetAccountsByUserId(string userId)
        {
            return await _context.Accounts
                .Where(a => a.UserId == userId)
                .Include(a => a.Transactions.Take(5))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Account>?> GetAccountsByUserEmail(string userEmail)
        {
            throw new NotImplementedException("Need user service integration");

        }

        // ____________________________________________

        public async Task<Account> CreateAccount(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> UpdateAccount(Account account)
        {
            var existingAccount = await _context.Accounts.FindAsync(account.Id);
            if (existingAccount == null) return null;

            existingAccount.Balance = account.Balance;
            existingAccount.AccountType = account.AccountType;

            await _context.SaveChangesAsync();
            return existingAccount;
        }

        public async Task<bool> ToggleAccountStatus(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.IsActive = !account.IsActive; 
            
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool?> DeleteAccount(int id)
        //{

        //    return null;
        //}


        // ____________________________________________

        public async Task<decimal> GetBalance(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);

            return account?.Balance ?? 0;
        }

        // ____________________________________________

        public async Task<bool> AccountExistsAsync(int accountId, string userId)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Id == accountId && a.UserId == userId);
        }


        public DbContext GetDbContext() => _context; 


    }
}
