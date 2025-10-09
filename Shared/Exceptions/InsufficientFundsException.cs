
namespace Banking.Shared.Exceptions
{
    public class InsufficientFundsException: Exception
    {
        public decimal RequiredAmount { get; set; }
        public decimal AvailableAmount { get; set; }

        public InsufficientFundsException(decimal requiredAmount, decimal availableAmount)
            : base($"Insuffisant funds, Required: {requiredAmount:C}, Available {availableAmount:C}")
        {
            RequiredAmount = requiredAmount;
            AvailableAmount = availableAmount;
        }

        public InsufficientFundsException(string message) 
            : base (message) { } 

    }
}
