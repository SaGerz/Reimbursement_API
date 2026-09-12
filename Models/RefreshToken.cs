namespace Reimbursement_API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public int UserId { get; set;  }
        public User User { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool isRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
