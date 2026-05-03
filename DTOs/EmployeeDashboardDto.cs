using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class EmployeeDashboardDto
    {
        public int totalReimburstment {get; set;}
        public decimal totalAmount  {get;  set;}
        public int totalApprove {get; set;}
        public int totalRejected {get; set;}
        public int totalPending {get; set;}
        public decimal totalPaid {get; set;}
        public List<RecentReimburstmentDto> Recent{get; set;}
    }
}