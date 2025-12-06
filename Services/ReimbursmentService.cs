using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Models;
using Reimbursement_API.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<ReimburstmentListDto>> GetMyReimburstmentAsync(int userId)
        {
            var results = await _context.Reimburstments
                .Where(r => r.EmployeeId == userId)
                .Include(r => r.Category)
                .OrderByDescending(r => r.CreateAt)
                .Select(r => new ReimburstmentListDto
                {
                    ReimbursementId = r.ReimbursementId,
                    Description = r.Description,
                    Amount = r.Amount,
                    CategoryName = r.Category.CategoryName,
                    ExpeseDate = r.ExpenseDate,
                    Status = r.Status,
                    ReceiptAttachment = r.ReceiptAttachment, 
                    CreateAt = r.CreateAt
                }).ToListAsync();
        
            return results;        
        }

        public async Task<ReimburstmentDetailDto?> GetDetailAsync(int id, int currentUserId)
        {
            var data = await _context.Reimburstments
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.ReimbursementId == id);

            if(data == null)
                return null;

            if(data.EmployeeId != currentUserId)
            {
                return new ReimburstmentDetailDto
                {
                    ReimbursementId = -1
                };
            }

            return new ReimburstmentDetailDto
            {
                ReimbursementId = data.ReimbursementId,
                Description = data.Description,
                Amount = data.Amount,
                CategoryName = data.Category.CategoryName,
                ExpenseDate = data.ExpenseDate,
                Status = data.Status,
                ReceiptAttachment = data.ReceiptAttachment,
                CreateAt = data.CreateAt,
                // ApproverName = data.Approver?.FullName,
                // ApprovedAt = data.ApprovedAt,
                // RejectReason = data.RejectReason
            };
        }

        public async Task<List<PendingReimburstmentDto>> GetPendingReimburstmentAsync()
        {
            var data = await _context.Reimburstments
                .Include(r => r.Employee)
                .Include(r => r.Category)
                .Where(r => r.Status == "Pending")  
                .OrderByDescending(r => r.CreateAt)
                .ToListAsync();

            return data.Select(r => new PendingReimburstmentDto
            {
                ReimburstmentId = r.ReimbursementId,
                EmployeeName = r.Employee.FullName,
                CategoryName = r.Category.CategoryName,
                ReimburstmentStatus = r.Status,
                CreateAt = r.CreateAt
            }).ToList();
        }

        public async Task<ReimburstmentManagerDetailDto> GetDetailReimburstmentManagerAsync(int id)
        {
            var data = await _context.Reimburstments
                .Include(r => r.Employee)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.ReimbursementId == id);

            return new ReimburstmentManagerDetailDto
            {
                ReimbursementId = data.ReimbursementId,
                EmployeName = data.Employee.FullName,
                CategoryName = data.Category.CategoryName,
                Description = data.Description,
                ReceiptAttachment = data.ReceiptAttachment,
                ReimburstmentStatus = data.Status,
                Amount = data.Amount,
                CreateAt = data.CreateAt,
                ExpenseDate = data.ExpenseDate
            };
        }
    }
}