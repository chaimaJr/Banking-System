using System.ComponentModel.DataAnnotations;

namespace AccountService.DTOs
{
    public class DepositWithdrawDto
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}
