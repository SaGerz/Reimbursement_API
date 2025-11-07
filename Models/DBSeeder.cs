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
            bool isChanged = false;

            // === USERS SEED ===
            if (!context.Users.Any(u => u.Email == "manager@company.com"))
            {
                context.Users.Add(new User
                {
                    FullName = "Manager Default",
                    Email = "manager@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                    Role = "Manager"
                });
                isChanged = true;
            }

            if (!context.Users.Any(u => u.Email == "finance@company.com"))
            {
                context.Users.Add(new User
                {
                    FullName = "Finance Default",
                    Email = "finance@company.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("finance123"),
                    Role = "Finance"
                });
                isChanged = true;
            }

            // === CATEGORIES SEED ===
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryName = "Transport", Description = "Biaya perjalanan dan transportasi" },
                    new Category { CategoryName = "Meal", Description = "Makan siang, konsumsi meeting, dll" },
                    new Category { CategoryName = "Office Supplies", Description = "Alat tulis, printer, dan perlengkapan kantor" },
                    new Category { CategoryName = "Travel", Description = "Perjalanan dinas dan penginapan" }
                );
                isChanged = true;
            }

            // 🧾 SIMPAN KE DB KALAU ADA YANG BARU
            if (isChanged)
                context.SaveChanges();
        }
    }
}