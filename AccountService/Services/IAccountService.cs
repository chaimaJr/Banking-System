using AccountService.DTOs;
using AccountService.Models;

namespace AccountService.Services
{
    public interface IAccountService
    {
        // Account Management
        Task<bool> UserOwnsAccount(int accountId, string userId);
        Task<IEnumerable<Account>> GetUserAccounts(string userId);
        Task<Account?> GetAccountById(int accountId, string userId);
        Task<Account> CreateAccount(CreateAccountDto dto, string userId);
        Task<bool> ToggleAccountStatus(int accountId, string userId);
        Task<OperationResultDto> Deposit(int accountId, string userId, DepositWithdrawDto amountDto);
        Task<OperationResultDto> Withdraw(int accountId, string userId, DepositWithdrawDto amountDto);
        Task<decimal> GetBalance(int accountId, string userId);


    }
} 
