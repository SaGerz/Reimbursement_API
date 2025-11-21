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
using Reimbursement_API.Models;
using Microsoft.AspNetCore.Identity;

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

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> GetMyReimburstments()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return BadRequest(new { message = "User ID claim not found" });
                }
                
                var userId = int.Parse(userIdClaim.Value);

                var result = await _reimbursementServices.GetMyReimburstmentAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _reimbursementServices.GetDetailAsync(id, currentUserId);

            if(result == null)
                return NotFound(new {message = "Reimbursment Not Found!"});

            if(result.ReimbursementId == -1)
                return Unauthorized(new {message = "You do not have access to this reimburstment"});

            return Ok(result);
        }
    }
}