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

namespace Stomatology3.Repositories
{
    public class AuthRepository
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IJwtHandlerAuth _jwtHandlerAuth;
        private readonly RoleManager<IdentityRole> _roleManager;

        public  AuthRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IJwtHandlerAuth jwtHandlerAuth,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtHandlerAuth = jwtHandlerAuth;
            _roleManager = roleManager;
        }
        async public Task<IActionResult> LoginPostAsync(UserDto authUser)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == authUser.Email);


        }

    }
}
