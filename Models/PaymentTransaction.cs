using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reimbursement_API.Models
{
    public class PaymentTransaction
    {
        [Key]
        public int PaymentTransactionId { get; set; }
        
        [Required]
        public int ReimburstmentId { get; set; }
        [ForeignKey("ReimburstmentId")]
        public Reimburstment Reimburstment { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Status { get; set; }
        public string? Provider { get; set; }
        public string? ProviderRefrence { get; set; }
        public string? FailureReason { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompleteAt { get; set; }

    }
}
