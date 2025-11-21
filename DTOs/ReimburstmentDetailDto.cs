using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class ReimburstmentDetailDto
    {
        public int ReimbursementId {get; set;}
        public string Description {get; set;}
        public decimal Amount {get; set;}
        public string CategoryName {get; set;}
        public DateTime ExpenseDate {get; set;}
        public string Status {get; set;}
        public string ReceiptAttachment {get; set;}
        public DateTime CreateAt {get; set;}


        // Detail Data buat nanti : 
        // public string ApproverName { get; set; }
        // public DateTime? ApprovedAt { get; set; }
        // public string RejectReason { get; set; }
    }
}