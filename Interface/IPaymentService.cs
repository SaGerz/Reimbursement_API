using Reimbursement_API.DTOs;

namespace Reimbursement_API.Interface
{
    public interface IPaymentService
    {
        Task<bool> PayAsync(int financeUserId, int reimburstmentId);
        Task<bool> HandleWebHookAsync(PaymentWebHookDto dto);
    }
}
