using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Context;
using Stomatology3.Controllers.Users.UserViewModels;
using Stomatology3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stomatology3.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<GetUsersModel>> GetUsersListAsync()
        {
            var usersList = await _userManager.Users.ToListAsync();
            return usersList.AsEnumerable()
                .Select(x => new GetUsersModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address
                });
        }
        public async Task<GetUsersModel> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return new GetUsersModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }
        public async Task<GetUsersModel> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return new GetUsersModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }
        //public async Task<GetUsersModel> UpdateUserAsync(string id)
        //{

        //}
    }
}
