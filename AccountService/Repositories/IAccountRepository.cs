using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccountList();
        Task<Account?> GetAccountById(int id);
        Task<IEnumerable<Account>?> GetAccountsByUserId(string userId);
        Task<IEnumerable<Account>?> GetAccountsByUserEmail(string userEmail);

        // ____________________________________________

        Task<Account> CreateAccount(Account account);
        Task<Account?> UpdateAccount(Account account);
        Task<bool> ToggleAccountStatus(int accountId);
        //Task<bool?> DeleteAccount(int id);

        // ____________________________________________

        Task<decimal> GetBalance(int accountId);

        // ____________________________________________

        Task<bool> AccountExists(int accountId, string userId);
        DbContext GetDbContext();

    }
}
