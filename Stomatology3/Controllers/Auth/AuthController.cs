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
        //private readonly ClaimsPrincipal _principal;

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
            RoleManager<IdentityRole> roleManager)
            //ClaimsPrincipal principal)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtHandlerAuth = jwtHandlerAuth;
            _roleManager = roleManager; 
            //_principal = principal;
        }

        // POST api/<AuthController>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       async public Task<IActionResult> LoginPost(AuthUser authUser)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == authUser.Email);

            if (authUser is null) return BadRequest(AppResources.NullUser);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<IActionResult> RegisterPost(RegisterUser registerUser)
        {
            if (registerUser is null) return BadRequest(AppResources.NullUser);

            var user = _context.Users.FirstOrDefault(user => user.Email == registerUser.Email);

            if (user != null) return BadRequest(AppResources.UserAlreadyExists);

            var hasher = new PasswordHasher<RegisterUser>();
            var hash = hasher.HashPassword(registerUser, registerUser.Password);
            var newUser = new User
            {
                //Id = new Guid().ToString(),
                Email = registerUser.Email,
                //NormalizedEmail = registerUser.Email.ToLower(),
                PasswordHash = hash,                
                UserName = registerUser.Email,
                //NormalizedUserName = registerUser.Email.ToLower(),
                CreatedOn = DateTime.UtcNow,
                //FirstName = registerUser.FirstName,
                //LastName = registerUser.LastName,
                //FullName = $"{registerUser.FirstName} {registerUser.LastName}" ,
            };
            var result = await _userManager.CreateAsync(newUser);

            if (result.Succeeded)
            {   //Improve
                var assignedBasicRole = await _userManager.AddToRoleAsync(newUser, _roleManager.FindByNameAsync("Basic").Result.ToString());
                return Ok();
            }
            else return BadRequest(AppResources.UserRegistrationImpossible);
        }
        //public async Task<IActionResult> UpdateUserAuthDetailsAsync(RegisterUser registerUser)
        //{
        //    if (registerUser is null) return BadRequest(AppResources.NullUser);


        //}
    }
}
