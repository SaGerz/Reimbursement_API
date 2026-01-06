using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class FinanceReportEmployeeDto
    {
        public int EmployeeId {get; set;}
        public string EmployeeName {get; set;}
        public decimal TotalAmount {get; set;}
        public int TotalRequest {get; set;}
    }
}