using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reimbursement_API.DTOs;
using Reimbursement_API.Services;
using Reimbursement_API.Interface;

namespace Reimbursement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReimburstmentController : ControllerBase
    {
        private IReimbursementService _reimbursementServices;

        public ReimburstmentController(IReimbursementService reimbursementServices)
        {
            _reimbursementServices = reimbursementServices;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateReimbursement([FromForm] CreateReimburstmentDto dto)
        {
            try
            {
                var Email = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _reimbursementServices.CreateReimburstmentAsync(Email, dto);
                return Ok(new
                {
                    message = "Reimbursement created successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}