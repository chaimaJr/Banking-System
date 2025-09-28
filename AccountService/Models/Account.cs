using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required] public string UserId { get; set; }
        [Required] public string AccountNumber { get; set; }
        [Required] public AccountType AccountType { get; set; }
        [Required] public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required] public bool IsActive { get; set; }

        // Relations
        public IEnumerable<Transaction> Transactions { get; set; } = [];
        
    }

    public enum AccountType
    {
        Savings,
        Current
    }
}
