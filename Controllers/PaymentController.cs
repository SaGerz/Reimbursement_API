using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbursement_API.DTOs;
using Reimbursement_API.Interface;

namespace Reimbursement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("weebhook")]
        public async Task<IActionResult> WebHook([FromBody] PaymentWebHookDto paymentWebHookDto)
        {
            try
            {
                await _paymentService.HandleWebHookAsync(paymentWebHookDto);
                return Ok(new { message = "WebHook Processed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
