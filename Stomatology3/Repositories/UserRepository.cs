using Microsoft.AspNetCore.Identity;
using Stomatology3.Context;
using Stomatology3.Controllers.Users.UserViewModels;
using Stomatology3.Models;
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

        public async Task<GetUsersModel> GetUsersAsync()
        {
            var usersList = _userManager.Users.ToList();
            return usersList.AsEnumerable()
                .Select(x => new InitialGetUserModel
                {
                    UserName = x.UserName,
                }
        }
    }
}
