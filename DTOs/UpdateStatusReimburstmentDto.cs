using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class UpdateStatusReimburstmentDto
    {
        public string NewStatus {get; set;}
        public string ManagerNotes {get; set;}
    }
}