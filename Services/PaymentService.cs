using Microsoft.EntityFrameworkCore;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Interface;
using Reimbursement_API.Models;

namespace Reimbursement_API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        public PaymentService (AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> PayAsync(int financeUserId, int reimburstmentId)
        {
            var reimburstment = await _context.Reimburstments
                    .FirstOrDefaultAsync(x => x.ReimbursementId == reimburstmentId);

            if (reimburstment == null)
            {
                throw new Exception("Reimburstment tidak ditemukan!");
            }

            if(reimburstment.Status != "Approved")
            {
                throw new Exception("Reimburstment belum disetujui!");
            }

            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.UserId == reimburstment.EmployeeId && x.IsActive);

            if (bankAccount == null)
            {
                throw new Exception("Karyawan belum memiliki rekening!");
            }

            var payment = await _context.PaymentTransactions
                .FirstOrDefaultAsync(x => x.ReimburstmentId == reimburstmentId);

            if (payment != null)
            {
                throw new Exception("Reimburstment sudah pernah melakukan proses payment!");
            }

            var paymentTransaction = new PaymentTransaction
            {
                ReimburstmentId = reimburstment.ReimbursementId,
                Status = "Pending",
                Amount = reimburstment.Amount,
                CreateAt = DateTime.UtcNow
            };

            _context.PaymentTransactions.Add(paymentTransaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HandleWebHookAsync(PaymentWebHookDto dto)
        {
            var paymentCheckExist = await _context.PaymentTransactions
                .FirstOrDefaultAsync(x => x.PaymentTransactionId == dto.PaymentTransactionId);

            if(paymentCheckExist == null)
            {
                throw new Exception("Payment transaction belum pernah dibuat!");
            }

            if(paymentCheckExist.Status == "Success")
            {
                return true;
            }
            
            if(dto.PaymentStatus == "Success")
            {
                paymentCheckExist.Status = "Success";
                paymentCheckExist.CompleteAt = DateTime.UtcNow;
            } else
            {
                throw new Exception("Payment Mock Failed");
            }

            var reimburstment = await _context.Reimburstments
                .FirstOrDefaultAsync(x => x.ReimbursementId == paymentCheckExist.ReimburstmentId);
            
            if(reimburstment == null)
            {
                throw new Exception("Reimburstment belum terbentuk");
            }

            reimburstment.Status = "Paid";
            reimburstment.PaidDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
