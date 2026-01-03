using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class FinancePaymentQueueDto
    {
        public int ReimbursementId {get; set;}
        public string EmployeeName {get; set;}
        public decimal Amount {get; set;}
        public string CategoryName {get; set;}
        public DateTime ApproveAt {get; set;}
    }
}