using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Data;

namespace Reimbursement_API.Models
{
    public class DBSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if(!context.Users.Any())
            {
                context.Users.AddRange(
                new User { FullName = "Manager Default", Email = "manager@company.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"), Role = "Manager" },
                new User { FullName = "Finance Default", Email = "finance@company.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("finance123"), Role = "Finance" }                );
            }
        }
    }
}