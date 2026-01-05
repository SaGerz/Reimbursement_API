using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class FinanceDashboardDto
    {
        public decimal TotalPaidThisMonth {get; set;}
        public int TotalPaidCountThisMonth {get; set;}
    }
}