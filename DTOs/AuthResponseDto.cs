using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_API.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}