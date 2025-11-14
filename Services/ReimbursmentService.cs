using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Models;
using Reimbursement_API.Interface;

namespace Reimbursement_API.Services
{
    public class ReimbursmentService : IReimbursementService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ReimbursmentService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<Reimburstment> CreateReimburstmentAsync(string Email, CreateReimburstmentDto dto)
        {
            var employee = _context.Users.FirstOrDefault(u => u.Email == Email);
            Console.WriteLine($"Employee Id : {employee.UserId}");

            string? receiptPath = null;

            if (dto.ReceiptAttachment != null)
            {
                var uploadFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", "receipts");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ReceiptAttachment.FileName)}";
                var fullPath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.ReceiptAttachment.CopyToAsync(stream);
                }

                receiptPath = Path.Combine("uploads", "receipts", fileName);
            }    

            // ✅ 2. Buat object baru
            var reimbursement = new Reimburstment
            {
                EmployeeId = employee.UserId,
                ExpenseDate = dto.ExpenseDate,
                Description = dto.Description,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
                Status = "Pending",
                ReceiptAttachment = receiptPath,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            // ✅ 3. Simpan ke database
            _context.Reimburstments.Add(reimbursement);
            await _context.SaveChangesAsync();

            return reimbursement;
        }
    }
}