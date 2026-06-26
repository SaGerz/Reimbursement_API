using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reimbursement_API.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string BankCode { get; set; }

        [Required]
        public string Accountnumber { get; set; }

        [Required]
        public string AccountHolderName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
