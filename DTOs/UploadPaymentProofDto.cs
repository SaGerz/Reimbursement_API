using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class UploadPaymentProofDto
    {
        [Required]
        public IFormFile PaymentAttachment {get; set;}
    }
}