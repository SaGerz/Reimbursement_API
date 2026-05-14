using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class FinanceDashboardDto
    {
        public int TotalPendingPayment { get; set; }
        public decimal TotalAmountPending { get; set; }
        public int PaidThisMount { get; set; }
        public decimal TotalPaidThisMonth {get; set;}
        public int? TotalRejectThisMonth { get; set; }
    }
}