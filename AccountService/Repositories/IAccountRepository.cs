using AccountService.Models;

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
        Task<Account?> ToggleAccountStatus(int accountId);
        //Task<bool?> DeleteAccount(int id);

        // ____________________________________________

        Task<decimal> GetBalance(int accountId);

        // ____________________________________________

        Task<bool> AccountExistsAsync(int accountId, string userId);

    }
}
