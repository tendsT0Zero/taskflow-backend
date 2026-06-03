using App.Application.DTOs;
using App.Application.DTOs.Common;
using App.Application.Interfaces;
using App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;


        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists == null)
                {
                    return new ApiResponse<AuthResponseDto>()
                    {
                        Success = false,
                        Message = "Invalid email or password.",
                        Errors = new List<string> { "User not found." }
                    };
                }

                var token = GenerateJwtToken(userExists);

                return new ApiResponse<AuthResponseDto>()
                {
                    Success = true,
                    Message = "Login successful.",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "An error occurred during login.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User already exists!",
                    Errors = new List<string> { "Email is already registered." }
                };
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User registration failed.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new ApiResponse<string> { 
                Success = true, 
                Message = "User created successfully!", 
                Data = user.Id.ToString() 
            };
        }


        private AuthResponseDto GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("FullName", user.FullName)
            };

            var expiryMinutes = Convert.ToDouble(jwtSettings["ExpiryMinutes"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(securityToken),
                FullName = user.FullName,
                Email = user.Email!,
                ExpiresAt = tokenDescriptor.Expires.Value
            };

        }
    }
}
