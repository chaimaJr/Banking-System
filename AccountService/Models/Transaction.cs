using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required] public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Timestamp { get; set; }

        // Relations
        [Required] public int AccountId { get; set; }
        [Required] public Account Account { get; set; }


    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
