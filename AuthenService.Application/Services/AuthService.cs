using AuthenService.Application.DTOs;
using AuthenService.Application.Interfaces;
using AuthenService.Application.Utils;
using AuthenService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AuthenService.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtSettings _jwtSettings;
        public AuthService(UserManager<User> userManager, ILogger<AuthService> logger, IOptions<JwtSettings> JwtSettings)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtSettings = JwtSettings.Value;

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            
            {
                user.AccessFailedCount++;
                await _userManager.UpdateAsync(user);
                throw new Exception("Invalid credentials");
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = GenerateJwtToken(user, "User");
            _logger.LogInformation("User {Username} logged in successfully", user.UserName);
            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                }
            };
        }


        private string GenerateJwtToken(User user, string role)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

                var securityToken = new JwtSecurityToken
             (
                 issuer: _jwtSettings.Issuer,
                 audience: _jwtSettings.Audience,
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.DurationInMinutes)),
                 signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
             );
                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                return (token);
        }

    }



}
