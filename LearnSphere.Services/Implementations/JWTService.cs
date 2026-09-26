
using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration  _conf;
        public JWTService(IConfiguration conf)
        {
            _conf = conf;
        }
        public async Task<string> GenerateJWTToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["Auth:JWT:Key"]?? "this is my secret key it must be very secure and hidden"));

            var crds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var clims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Role,user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            };
            if (!int.TryParse(_conf["Auth:JWT:DurationInMinuts"],out int minuts))
            {
                minuts = 60;
            }
            var token = new JwtSecurityToken(
                signingCredentials: crds,
                issuer: _conf["Auth:JWT:Issuer"],
                audience: _conf["Auth:JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(minuts),
                claims: clims


                );
            return new JwtSecurityTokenHandler().WriteToken(token);
         

        }
    }
}
