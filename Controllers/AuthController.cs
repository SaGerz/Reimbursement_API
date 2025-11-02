using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Models;
using System.IdentityModel.Tokens.Jwt;        // JwtSecurityToken, JwtSecurityTokenHandler
using System.Security.Claims;                // Claim, ClaimTypes
using System.Text;                           // Encoding
using Microsoft.IdentityModel.Tokens;
using Reimbursement_API.Services;        // SymmetricSecurityKey, SigningCredentials, SecurityAlgorithms


namespace Reimbursement_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, IConfiguration config, AuthService authService)
        {
            _context = context;
            _config = config;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var result = _authService.RegisterAsync(dto);
                return Ok(new { message = result });
            } catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}