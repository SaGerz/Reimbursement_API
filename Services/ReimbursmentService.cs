using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Models;
using Reimbursement_API.Interface;
using Microsoft.EntityFrameworkCore;
using Reimbursement_API.Helpers;

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

        public async Task<Reimburstment> CreateReimburstmentAsync(int id, CreateReimburstmentDto dto)
        {
            var employee = _context.Users.FirstOrDefault(u => u.UserId == id);
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

        public async Task<PaginationResponse<ReimburstmentListDto>> GetMyReimburstmentAsync(int userId, int page, int pageSize)
        {
            var query = _context.Reimburstments
                .Where(r => r.EmployeeId == userId)
                .Include(r => r.Category)
                .OrderByDescending(r => r.CreateAt);
                
            var totalCount = await query.CountAsync();

            var data = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
            
            var result = data.Select(r => new ReimburstmentListDto
                {
                    ReimbursementId = r.ReimbursementId,
                    Description = r.Description,
                    Amount = r.Amount,
                    CategoryName = r.Category.CategoryName,
                    ExpeseDate = r.ExpenseDate,
                    Status = r.Status,
                    ReceiptAttachment = r.ReceiptAttachment, 
                    CreateAt = r.CreateAt
                }).ToList();
        
            return new PaginationResponse<ReimburstmentListDto>
            {
                Data = result,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double) totalCount / pageSize)
            };        
        }

        public async Task<EmployeeDashboardDto> GetEmployeeDashboardAsync(int userId)
        {
            var data = await _context.Reimburstments
                .Where(r => r.EmployeeId == userId)
                .Include(r => r.Category)
                .ToListAsync();

            return new EmployeeDashboardDto
            {
                totalReimburstment = data.Count,
                totalAmount = data.Sum(x => x.Amount),
                totalApprove = data.Count(x => x.Status == "Approved"),
                totalRejected = data.Count(x => x.Status == "Rejected"),
                totalPending = data.Count(x => x.Status == "Pending"),
                totalPaid = data.Where(x => x.Status == "Paid").Sum(x => x.Amount),
                Recent = data
                            .OrderByDescending(x => x.CreateAt)
                            .Take(5)
                            .Select(x => new RecentReimburstmentDto
                            {
                                ReimbursementId = x.ReimbursementId,
                                CreateAt = x.CreateAt,
                                CategoryName = x.Category.CategoryName,
                                Description = x.Description,
                                Amount = x.Amount,
                                Status = x.Status
                            }).ToList()
            };
        }

        public async Task<ReimburstmentDetailDto?> GetDetailAsync(int id, int currentUserId)
        {
            var data = await _context.Reimburstments
                .Include(r => r.Category)
                .Include(r => r.PaidByUser)
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
                PaidBy = data.PaidBy,
                PaidByName = data.PaidByUser?.FullName,
                PaidDate = data.PaidDate,
                PaymentAttachment = data.PaymentAttachment,
                // ApproverName = data.Approver?.FullName,
                // ApprovedAt = data.ApprovedAt,
                RejectReason = data.RejectedReason
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
                CreateAt = r.CreateAt,
                ReceiptAttachment = r.ReceiptAttachment,
                Amount = r.Amount,
                Description = r.Description
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

        public async Task<bool> ApproveAsync(int userId, int id, string? ManagerApproveNotes)
        {
            var reimbursement = await _context.Reimburstments.FirstOrDefaultAsync(r => r.ReimbursementId == id);

            if(reimbursement == null)
            {
                return false;
            }   

            reimbursement.Status = "Approved";
            reimbursement.ApprovedBy = userId;
            reimbursement.ApprovedAt = DateTime.Now;

            await AddHistory(id, userId, "Approved", ManagerApproveNotes);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> RejectAsync(int userId, int id, string ManagerRejectedNotes)
        {
            var reimbursement = await _context.Reimburstments.FirstOrDefaultAsync(r => r.ReimbursementId == id);

            if(reimbursement == null)
            {
                return false;
            }

            reimbursement.Status = "Rejected";
            reimbursement.RejectedReason = ManagerRejectedNotes;
            
            await AddHistory(id, userId, "Rejected", ManagerRejectedNotes);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper : 
        public async Task AddHistory(int reimburstmentId, int userId, string actionType, string remarks)
        {
            var approvalHistory = new ApprovalHistory
            {
                ReimbursementId = reimburstmentId,
                ActionBy = userId,
                ActionDate = DateTime.Now,
                ActionType = actionType, 
                Remarks = remarks
            };

            await _context.ApprovalHistories.AddAsync(approvalHistory);
        }

        public async Task<PaginationResponse<ApprovalHistoryDto>> GetApprovalHistoryAsync(int page, int pageSize)
        {
            var query = _context.ApprovalHistories
                                .Include(h => h.User)
                                .Include(h => h.Reimburstment).ThenInclude(r => r.Employee)
                                .OrderByDescending(h => h.ActionDate);

            var totalCount = await query.CountAsync();

            var data = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var result = data.Select(h => new ApprovalHistoryDto
            {
                ReimburstmentID = h.ReimbursementId,
                CreatedBy = h.Reimburstment.Employee.FullName,
                ActionBy = h.User.FullName,
                ActionType = h.ActionType,
                ActionDate = h.ActionDate,
                Remarks = h.Remarks
            }).ToList();

            return new PaginationResponse<ApprovalHistoryDto>
            {
                Data = result,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync()
        {
            var now = DateTime.UtcNow;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var totalPendingThisMoth = await _context.Reimburstments
                .CountAsync(x => x.Status == "Pending" 
                            && x.UpdateAt.Month == currentMonth
                            && x.UpdateAt.Year == currentYear);

            var totalApproveThisMonth = await _context.Reimburstments
                .CountAsync(x => x.Status == "Approved"
                            && x.UpdateAt.Month == currentMonth
                            && x.UpdateAt.Year == currentYear
                );

            var totalRejectedThisMonth = await _context.Reimburstments
                .CountAsync(x => x.Status == "Rejected"
                            && x.UpdateAt.Month == currentMonth
                            && x.UpdateAt.Year == currentYear
                );

            var totalRequestThisMonth = await _context.Reimburstments
                .CountAsync(x => x.UpdateAt.Month == currentMonth && x.UpdateAt.Year == currentYear);

            return new ManagerDashboardDto
            {
                TotalPendingThisMonth = totalPendingThisMoth,
                TotalApproveThisMonth = totalApproveThisMonth,
                TotalRejectedThisMonth = totalRejectedThisMonth,
                TotalRequestThisMonth = totalRequestThisMonth
            };
            
        }

        public async Task<PaginationResponse<FinancePaymentQueueDto>> GetPaymentQueueAsync(int page, int pageSize)
        {
            var query = _context.Reimburstments
                .Where(r => r.Status == "Approved" && r.PaidBy == null)
                .Include(r => r.Employee)
                .Include(r => r.Category)
                .OrderByDescending(r => r.ApprovedAt);

            var totalCount = await query.CountAsync();

            var data = await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var result = data.Select(r => new FinancePaymentQueueDto
            {
                ReimbursementId = r.ReimbursementId,
                CategoryName = r.Category.CategoryName,
                EmployeeName = r.Employee.FullName,
                Amount = r.Amount,
                ApproveAt = r.ApprovedAt,
                ReceiptAttachment = r.ReceiptAttachment,
                DescriptionReimburstment = r.Description
            }).ToList();

            return new PaginationResponse<FinancePaymentQueueDto>
            {
                Data = result,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<bool> UploadPaymentProofAsync(int financeUserId, int reimbursementId, UploadPaymentProofDto dto)
        {
            var reimbursement = await _context.Reimburstments.FirstOrDefaultAsync(r => r.ReimbursementId == reimbursementId);

            if(reimbursement == null)
            {
                throw new Exception("Reimburstment tidak ditemukan!");
            }    

            if(reimbursement.Status != "Approved")
            {
                throw new Exception("Reimburstment Belum disetujui Manager!");
            }

            string? paymentPath = null;

            if(dto.PaymentAttachment != null)
            {
                var uploadFolder = Path.Combine(
                    _environment.WebRootPath ?? "wwwroot", "uploads", "payments"
                );

                if(!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PaymentAttachment.FileName)}";
                var fullPath = Path.Combine(uploadFolder, fileName);

                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.PaymentAttachment.CopyToAsync(stream);
                }

                paymentPath = Path.Combine("uploads", "payments", fileName);
            }

            reimbursement.PaymentAttachment = paymentPath;
            reimbursement.PaidBy = financeUserId;
            reimbursement.PaidDate = DateTime.UtcNow;
            reimbursement.Status = "Paid";
            reimbursement.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FinanceDashboardDto> GetFinanceDashboardAsync()
        {
            var now = DateTime.UtcNow;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var totalPendingPayment = await _context.Reimburstments.CountAsync(x => x.Status == "Approved");
            
            var totalAmountPending = await _context.Reimburstments
                .Where(x => x.Status == "Approved")
                .SumAsync(x => x.Amount);

            var totalPaidThisMonth = await _context.Reimburstments
                .CountAsync(x =>
                    x.Status == "Paid" &&
                    x.UpdateAt.Month == currentMonth &&
                    x.UpdateAt.Year == currentYear
                );

            var totalAmountPaid = await _context.Reimburstments
                .Where(x => x.Status == "Paid")
                .SumAsync(x => x.Amount);

            return new FinanceDashboardDto
            {
               TotalPendingPayment = totalPendingPayment,
               TotalAmountPending = totalAmountPending,
               PaidThisMount = totalPaidThisMonth,
               TotalPaidThisMonth = totalAmountPaid
            };
        }

        public async Task<List<FinanceReportEmployeeDto>> GetReportByEmployeeAsync(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var report = await _context.Reimburstments
                .Where(r => 
                    r.Status == "Paid" &&
                    r.PaidDate >= startDate &&
                    r.PaidDate < endDate
                )
                .GroupBy(r => new
                {
                    r.EmployeeId,
                    r.Employee.FullName
                })
                .Select(g => new FinanceReportEmployeeDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.FullName,
                    TotalAmount = g.Sum(x => x.Amount),
                    TotalRequest = g.Count()
                })
                .OrderByDescending(r => r.TotalAmount)
                .ToListAsync();

            return report;
        }
    }
}