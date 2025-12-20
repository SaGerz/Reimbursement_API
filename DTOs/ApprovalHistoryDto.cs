using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class ApprovalHistoryDto
    {
        public int ReimburstmentID {get; set;}
        public string CreatedBy {get; set;}
        public string ActionBy {get; set;}
        public string ActionType {get; set;}
        public string Remarks {get; set;}
        public DateTime ActionDate {get; set;}
    }
}