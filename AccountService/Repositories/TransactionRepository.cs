using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository (ApplicationDbContext context) {  _context = context; }

        public async Task<Transaction?> GetTransactionById(int id)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByAccountId(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }
        public async Task<IEnumerable<Transaction>?> GetTransactionsByUserId(string userId)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.UserId == userId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByDateRange(int accountId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId
                    && t.Timestamp >= startDate
                    && t.Timestamp <= endDate)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }


        // _________________________________

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
        public async Task<Transaction?> UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = await _context.Transactions.FindAsync(transaction.Id);
            if (existingTransaction == null) return null;

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.TransactionType = transaction.TransactionType;

            await _context.SaveChangesAsync();
            return existingTransaction;
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        // _________________________________

        public async Task<decimal> GetTotalAmount(int accountId, TransactionType? type = null)
        {
            //Transaction list
            var query = _context.Transactions.Where(t => t.AccountId == accountId);

            if(type.HasValue) 
                query = query.Where(t => t.TransactionType == type.Value);

            return await query.SumAsync(t => t.Amount);
        }


    }
}
