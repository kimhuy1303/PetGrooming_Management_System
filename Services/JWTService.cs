using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Config.Constant;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetGrooming_Management_System.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string generateJwtToken(string userId, Role role, string phoneNumber)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.MobilePhone, phoneNumber.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    _configuration["jwt:Issuer"],
                    _configuration["jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signin
                );
            string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenvalue;
        }
    }
}
