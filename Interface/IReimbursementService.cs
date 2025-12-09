using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reimbursement_API.Models;
using Reimbursement_API.Controllers;
using Reimbursement_API.DTOs;

namespace Reimbursement_API.Interface
{
    public interface IReimbursementService
    {
        Task<Reimburstment> CreateReimburstmentAsync(string Email, CreateReimburstmentDto dto);
        Task<List<ReimburstmentListDto>> GetMyReimburstmentAsync(int userId);
        Task<ReimburstmentDetailDto?> GetDetailAsync(int id, int currentUserId);

        Task<List<PendingReimburstmentDto>> GetPendingReimburstmentAsync();
        Task<ReimburstmentManagerDetailDto> GetDetailReimburstmentManagerAsync(int id);
        Task<bool> UpdateReimburstmentStatusAsync(int userId, int id, UpdateStatusReimburstmentDto dto); 
    }
}