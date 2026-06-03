
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sarhne.BLL.Abstraction;
using Sarhne.BLL.DTOs.Auth;
using Sarhne.BLL.DTOs.Token;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sarhne.BLL.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly JwtInformations _jwt;

        public JwtService(IOptions<JwtInformations> options)
        {
            _jwt = options.Value;
        }
        public async Task<Response<GenerateTokenResDto>> GenerateToken(User user, IList<string> roles)
        {
            List<Claim> UserClaims = new List<Claim>();
            UserClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (var role in roles)
            {
                UserClaims.Add(new Claim(ClaimTypes.Role, role));
            }
           
            var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var SignInCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken userToken = new JwtSecurityToken(
                audience: _jwt.AudienceIP,
                issuer: _jwt.IssuerIP,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: SignInCred,
                claims: UserClaims
             );
            var data = new GenerateTokenResDto {
            Token =new JwtSecurityTokenHandler().WriteToken(userToken),
            ExpireIn = DateTime.UtcNow.AddHours(1),
            };
            
            return Response<GenerateTokenResDto>.Success(data);
        }
    }
}
