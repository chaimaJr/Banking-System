namespace AuthService.DTOs
{
    public class AuthResponseDto
    {
        // this what the API sends back to the Frontend after login
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty; // Success/Error message
    }
}
