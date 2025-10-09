using AccountService.DTOs;
using AccountService.Models;
using AccountService.Repositories;
using Banking.Shared.Exceptions;

namespace AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo ;
        private readonly ITransactionService _transactionService;

        
        public AccountService(IAccountRepository accountRepository, ITransactionService transactionService) 
        { 
            _accountRepo = accountRepository;
            _transactionService = transactionService;
        }

        public Task<bool> UserOwnsAccount(int accountId, string userId)
        {
            return _accountRepo.AccountExists(accountId, userId);
        }

        public async Task<IEnumerable<Account>> GetUserAccounts(string userId)
        {
            var accounts = await _accountRepo.GetAccountsByUserId(userId);
            return accounts ?? [];
        }
        
        public async Task<Account?> GetAccountById(int accountId, string userId)
        {
            if (!await UserOwnsAccount(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            return await _accountRepo.GetAccountById(accountId);
        }


        public async Task<Account> CreateAccount(CreateAccountDto dto, string userId)
        {
            var account = new Account
            {
                UserId = userId,
                AccountNumber = GenerateAccountNumber(),
                AccountType = dto.AccountType,
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };

            return await _accountRepo.CreateAccount(account);
        }


        public async Task<bool> ToggleAccountStatus(int accountId, string userId)
        {
            if (!await UserOwnsAccount(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            return await _accountRepo.ToggleAccountStatus(accountId);
        }



        public async Task<OperationResultDto> Deposit(int accountId, string userId, DepositWithdrawDto amountDto)
        {
            if (!await UserOwnsAccount(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            if (amountDto.Amount <= 0)
                throw new BusinessRuleException("Deposit amount must be positive.");

            var account = await _accountRepo.GetAccountById(accountId)
                ?? throw new NotFoundException("Account not found");

            if (!account.IsActive)
                throw new NotFoundException("Account is inactive");

            await using var dbTransaction = await _accountRepo.GetDbContext()
                .Database
                .BeginTransactionAsync();

            try
            {
                var transaction = new Transaction
                {
                    Amount = amountDto.Amount,
                    TransactionType = TransactionType.Deposit,
                    Timestamp = DateTime.UtcNow,
                    AccountId = accountId,
                };
                var createdTransaction = await _transactionService.CreateTransaction(transaction);

                account.Balance += amountDto.Amount;
                await _accountRepo.UpdateAccount(account);

                await dbTransaction.CommitAsync();

                return new OperationResultDto
                {
                    Message = "The deposit was successful",
                    NewBalance = account.Balance,
                    TransactionId = createdTransaction.Id
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OperationResultDto> Withdraw(int accountId, string userId, DepositWithdrawDto amountDto)
        {
            if (!await UserOwnsAccount(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            var account = await _accountRepo.GetAccountById(accountId)
                ?? throw new NotFoundException("Account not found");
                
            if (!account.IsActive)
                throw new NotFoundException("Account is inactive");

            if (amountDto.Amount <= 0)
                throw new BusinessRuleException("Withdrawal amount must be positive.");

            if (account.Balance < amountDto.Amount)
                throw new InsufficientFundsException(amountDto.Amount, account.Balance);

            await using var dbTransaction = await _accountRepo.GetDbContext()
                .Database
                .BeginTransactionAsync();

            try
            {
                var transaction = new Transaction
                {
                    Amount = amountDto.Amount,
                    TransactionType = TransactionType.Withdrawal,
                    Timestamp = DateTime.UtcNow,
                    AccountId = accountId,
                };

                var createdTransaction = await _transactionService.CreateTransaction(transaction);

                account.Balance -= amountDto.Amount;
                await _accountRepo.UpdateAccount(account);

                await dbTransaction.CommitAsync();

                return new OperationResultDto
                {
                    Message = "The withdrawal was successful",
                    NewBalance = account.Balance,
                    TransactionId = createdTransaction.Id
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }


        public async Task<decimal> GetBalance(int accountId, string userId)
        {
            if (!await UserOwnsAccount(accountId, userId))
                throw new NotFoundException($"Account with ID {accountId} not found or access denied");

            return await _accountRepo.GetBalance(accountId);
        }



        // ======================

        private static string GenerateAccountNumber()
        {
            return $"ACC-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
