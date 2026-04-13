using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Models;
using Reimbursement_API.Controllers;
using Reimbursement_API.DTOs;
using Reimbursement_API.Helpers;

namespace Reimbursement_API.Interface
{
    public interface IReimbursementService
    {
        // Karyawan
        Task<Reimburstment> CreateReimburstmentAsync(int id, CreateReimburstmentDto dto);
        Task<PaginationResponse<ReimburstmentListDto>> GetMyReimburstmentAsync(int userId, int page, int pageSize);
        Task<ReimburstmentDetailDto?> GetDetailAsync(int id, int currentUserId);

        // Manager
        Task<List<PendingReimburstmentDto>> GetPendingReimburstmentAsync();
        Task<ReimburstmentManagerDetailDto> GetDetailReimburstmentManagerAsync(int id);
        Task<bool> ApproveAsync(int userId, int id, string? ManagerApproveNotes);
        Task<bool> RejectAsync(int userId, int id, string ManagerRejectedNotes);
        Task<PaginationResponse<ApprovalHistoryDto>> GetApprovalHistoryAsync(int page, int pageSize);

        // Finance
        Task<FinanceDashboardDto> GetFinanceDashboardAsync();
        Task<PaginationResponse<FinancePaymentQueueDto>> GetPaymentQueueAsync(int page, int pageSize);
        Task<bool> UploadPaymentProofAsync(int financeUserId, int reimburstmentId, UploadPaymentProofDto dto);
        Task<List<FinanceReportEmployeeDto>> GetReportByEmployeeAsync(int month, int year);
    }
}