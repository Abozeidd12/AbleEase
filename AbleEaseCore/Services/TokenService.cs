using AbleEaseCore.IServices;
using AbleEaseInfrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(ApplicationUser applicationUser)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

            List<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, applicationUser.Name.ToString()),
                new Claim(ClaimTypes.Email,applicationUser.Email.ToString()),
                new Claim(ClaimTypes.Role,applicationUser.UserRole.ToString()),
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Id)




            };

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claim, issuer: _configuration["JWt:Issuer"], audience: _configuration["JWT:Audience"], expires: DateTime.UtcNow.AddDays(2),

                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
