using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class PendingReimburstmentDto
    {
        public int ReimburstmentId {get; set;}
        public string ReimburstmentStatus {get; set;}
        public string EmployeeName {get; set;}
        public string CategoryName {get; set;}
        public DateTime CreateAt {get; set;}
    }
}