using AccountService.Models;
using AccountService.Repositories;
using Banking.Shared.Exceptions;

namespace AccountService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;
        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepo = transactionRepository;
            _accountRepo = accountRepository;
        }



        public async Task<IEnumerable<Transaction>?> GetAccountTransactions(int accountId, string userId)
        {
            if (!await _accountRepo.AccountExists(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            var transactions =  await _transactionRepo.GetTransactionsByAccountId(accountId);
            return transactions ?? [];
        }

        public async Task<IEnumerable<Transaction>?> GetUserTransactions(string userId)
        {

            var transactions = await _transactionRepo.GetTransactionsByUserId(userId);
            return transactions ?? [];
        }

        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _transactionRepo.GetTransactionById(id)
                ?? throw new NotFoundException($"Transaction with ID {id} not found");
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRange(int accountId, string userId, DateTime startDate, DateTime endDate)
        {
            if (!await _accountRepo.AccountExists(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            if (startDate > endDate)
                throw new BusinessRuleException("Start date cannot be after end date");

            var transactions = await _transactionRepo.GetTransactionsByDateRange(accountId, startDate, endDate);
            return transactions ?? [];
        }

        

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            return await _transactionRepo.CreateTransaction(transaction);
        }

        public async Task<decimal> GetTotalAmount(int accountId, string userId, TransactionType? type = null)
        {
            if (!await _accountRepo.AccountExists(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            return await _transactionRepo.GetTotalAmount(accountId, type);
        }
    }
}
