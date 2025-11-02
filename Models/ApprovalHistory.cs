using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.Models
{
    public class ApprovalHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int ReimburstmentId { get; set; }
        [ForeignKey("ReimbursementId")]
        public Reimburstment Reimburstment { get; set; }

        [Required]
        public int ActionBy { get; set; }
        [ForeignKey("ActionBy")]
        public User User { get; set; }

        [Required]
        public string ActionType { get; set; }
        public string? Remarks { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

    }
}