namespace Reimbursement_API.DTOs
{
    public class PaymentWebHookDto
    {
        public int PaymentTransactionId { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
