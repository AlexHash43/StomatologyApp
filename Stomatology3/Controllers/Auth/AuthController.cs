using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stomatology3.Resources;
using Stomatology3.Context;
using Stomatology3.Controllers.Auth.AuthModels;
using Stomatology3.Interfaces;
using Stomatology3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
//using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Stomatology3.Controllers.Auth
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IJwtHandlerAuth _jwtHandlerAuth;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///     Authentication controller constructor. Takes:
        ///     <paramref name="userManager"/>
        ///     <paramref name="signInManager"/>,
        ///     <paramref name="context"/>,
        ///     <paramref name="iConfig"/> and
        ///     <paramref name="jwtHandlerAuth"/>.
        /// </summary>
        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IJwtHandlerAuth jwtHandlerAuth,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor
            )


        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtHandlerAuth = jwtHandlerAuth;
            _roleManager = roleManager; 
            _httpContextAccessor = httpContextAccessor;

            //_principal = principal;
        }

        // POST api/<AuthController>
        [AllowAnonymous]
        [HttpPost("login")]
       async public Task<IActionResult> LoginPost(UserDto authUser)
        {
            if (authUser is null) return BadRequest(AppResources.NullUser);
            var user = _context.Users.FirstOrDefault(user => user.Email == authUser.Email);
            if (user is null) return BadRequest(AppResources.UserBadCredentials);
            else
            {
                var validPassword = _userManager.CheckPasswordAsync(user, authUser.Password);

                if (!validPassword.Result) return BadRequest(AppResources.UserBadCredentials);

                var token = "";
                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    var tokenPart = _jwtHandlerAuth.Authentication(authUser, role.ToString());
                    token = token + tokenPart;
                }

                if (token == null) return BadRequest(AppResources.UserAuthenticationImpossible);

                string cookieValue = Request.Cookies["jwt"];

                var returnUser = new ReturnUser
                {
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                };

                if (cookieValue != token)
                {
                    Response.Cookies.Append("jwt", token, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                        Secure = true
                    });
                }

                var signInResult = await _signInManager.PasswordSignInAsync(authUser.Email, authUser.Password, false, false);

                if (!signInResult.Succeeded) return BadRequest(AppResources.UserAuthenticationImpossible);

                var context = _signInManager.Context;

                return Ok(returnUser);
            }
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        async public Task<IActionResult> RegisterPost(UserDto registerUser)
        {
            if (registerUser is null) return BadRequest(AppResources.NullUser);

            var user = _context.Users.FirstOrDefault(user => user.Email == registerUser.Email);

            if (user != null) return BadRequest(AppResources.NullUser);

            var hasher = new PasswordHasher<UserDto>();
            var hash = hasher.HashPassword(registerUser, registerUser.Password);
            var newUser = new User
            {
                Email = registerUser.Email,
                PasswordHash = hash,                
                UserName = registerUser.Email,
                CreatedOn = DateTime.UtcNow,

            };
            var result = await _userManager.CreateAsync(newUser);

            if (result.Succeeded)
            {   //Improve
                var assignedBasicRole = await _userManager.AddToRoleAsync(newUser, _roleManager.FindByNameAsync("Basic").Result.ToString());
                return Ok();
            }
            else return BadRequest(AppResources.UserRegistrationImpossible);
        }
        [AllowAnonymous]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword)
        {
            if (changePassword is null) return BadRequest(AppResources.NullUser);
            var user = await _userManager.FindByEmailAsync(changePassword.Email);
            var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            if (user == null) return BadRequest(AppResources.NullUser);
            if (string.Compare(user.Email, userName) == 0 )
            {
                if (string.Compare(changePassword.NewPassword, changePassword.ConfirmPassword) == 0)
                {
                    var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
                    if (!result.Succeeded)
                    {
                        var errors = new List<string>();
                        foreach (var error in result.Errors)
                        {
                            errors.Add(error.Description);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = string.Join(", ", errors) });
                    }
                    return Ok(new Response
                    {
                        Status = "Success",
                        Message = "Password successfully changed"
                    });
                }
            }
            else
            {
                return BadRequest("The new password and confirm doesn't match");
            }
            return Ok(new Response { Status = "Success", Message = "Password successfully changed." });
        }
    }
}
