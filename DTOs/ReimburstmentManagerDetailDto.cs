using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class ReimburstmentManagerDetailDto
    {
        public int ReimbursementId {get; set;}
        public string EmployeName {get; set;}
        public string CategoryName {get; set;}
        public string ReimburstmentStatus {get; set;}
        public string? ReceiptAttachment {get; set;}
        public string Description {get; set;}
        public decimal Amount {get; set;}
        public DateTime CreateAt {get; set;}
        public DateTime ExpenseDate {get; set;}
    }
}