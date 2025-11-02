using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.Models
{
    public class Reimburstment
    {
        [Key]
        public int ReimbursementId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public string? ReceiptAttachment { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }
        public string? RejectedReason { get; set; }

        public int? PaidBy { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PaymentAttachment { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}