namespace Reimbursement_API.DTO
{
    public class RefreshTokensResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
