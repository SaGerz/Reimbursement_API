using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reimbursement_API.Data;
using Reimbursement_API.DTOs;
using Reimbursement_API.Models;
using System.Text;                           // Encoding
using Microsoft.IdentityModel.Tokens;
using Reimbursement_API.DTO;

namespace Reimbursement_API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            Console.WriteLine("Masuk sini!");
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email Already registered");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Register Sucessful";
        }
        
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var token = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.UserId); 

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                Role = user.Role,
                FullName = user.FullName,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresMinutes"]))
            };
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiresDays"])),
                isRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Semacam Payload : 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "Employee") // role penting untuk authorize
            };

            // Bikin Object Token : 
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiresMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }    

        public async Task<RefreshTokensResponseDto> RefreshAccessTokenAsync(string refreshTokenValue)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue);

            if(storedToken == null || storedToken.isRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid or Expired refresh token!");
            }

            var newAccessToken = GenerateJwtToken(storedToken.User);

            return new RefreshTokensResponseDto
            {
                AccessToken = newAccessToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresMinutes"]))
            };

        }
    }
}