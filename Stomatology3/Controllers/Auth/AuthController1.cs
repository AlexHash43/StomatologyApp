using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stomatology3.Controllers.Auth.AuthModels;
using Stomatology3.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Stomatology3.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController1 : ControllerBase
    {
        public static ClaimUser user = new ClaimUser();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController1(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUser>> RegisterUserAsync(UserDto userToRegister)
        {
            CreatePasswordHash(userToRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Email = userToRegister.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUserASync(UserDto loginUser)
        {
            if (user.Email != loginUser.Email) return BadRequest("User not found.");
            if (!VerifyPasswordHash(loginUser.Password, user.PasswordHash, user.PasswordSalt)) return BadRequest("Wrong password");

            string token = CreateToken(user);
            return Ok("My Crazy Token");
        }
        private string CreateToken(ClaimUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name, user.FullName),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.Role, user.Role),
                //new Claim (ClaimTypes.NameIdentifier, user.Id),
            };

            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:PrivateKey").Value));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims : claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
