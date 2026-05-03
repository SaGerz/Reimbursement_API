using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class RecentReimburstmentDto
    {
        public int ReimbursementId {get; set;}
        public string Description {get; set;}
        public decimal Amount {get; set;}
        public string CategoryName {get; set;}
        public DateTime ExpeseDate {get; set;}
        public string Status {get; set;}
        public DateTime CreateAt {get; set;}
    }
}