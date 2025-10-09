using AccountService.DTOs;
using AccountService.Models;

namespace AccountService.Services
{
    public interface ITransactionService
    {
        // Transaction History
        Task<IEnumerable<Transaction>?> GetAccountTransactions(int accountId, string userId);
        Task<IEnumerable<Transaction>?> GetUserTransactions(string userId);
        Task<Transaction?> GetTransactionById(int id);

        // Transaction Analysis
        Task<IEnumerable<Transaction>> GetTransactionsByDateRange(int accountId, string userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalAmount(int accountId, string userId, TransactionType? type = null);

        // Internal Operations
        Task<Transaction> CreateTransaction(Transaction transaction);

    }
}
