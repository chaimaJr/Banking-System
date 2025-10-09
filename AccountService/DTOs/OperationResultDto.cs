namespace AccountService.DTOs
{
    public class OperationResultDto
    {
        public string Message { get; set; } = string.Empty;
        public decimal? NewBalance { get; set; }
        public int? TransactionId { get; set; }
    }
}
