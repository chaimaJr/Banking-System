using AccountService.Models;

namespace AccountService.Repositories
{
    public interface ITransactionRepository
    {

        Task<IEnumerable<Transaction>?> GetTransactionsByAccountId(int accountId);
        Task<IEnumerable<Transaction>?> GetTransactionsByUserId(string userId);
        Task<Transaction?> GetTransactionById(int id);
        Task<IEnumerable<Transaction>?> GetTransactionsByDateRange(int accountId, DateTime startDate, DateTime endDate);

        // _________________________________

        Task<Transaction> CreateTransaction(Transaction transaction);
        Task<Transaction?> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int id);

        // _________________________________

        Task<decimal> GetTotalAmount(int accountId, TransactionType? type = null);



    }
}
