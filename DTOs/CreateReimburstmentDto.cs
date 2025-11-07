using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class CreateReimburstmentDto
    {
        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } 
        
        [Required]
        public int CategoryId { get; set; }

        public IFormFile? ReceiptAttachment { get; set; }
    }
}