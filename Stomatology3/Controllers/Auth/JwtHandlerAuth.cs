using Microsoft.IdentityModel.Tokens;
using Stomatology3.Controllers.Auth.AuthModels;
using Stomatology3.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stomatology3.Controllers.Auth
{
    public class JwtHandlerAuth : IJwtHandlerAuth
    {
        private string _privateKey;

        public JwtHandlerAuth(string privateKey)
        {
            _privateKey = privateKey;
        }

        public string Authentication(AuthUser authUser, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_privateKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Check
            tokenHandler.WriteToken(token);
            return tokenHandler.WriteToken(token);

        }
    }
}
