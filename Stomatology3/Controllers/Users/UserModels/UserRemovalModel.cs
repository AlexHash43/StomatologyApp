using System.Collections.Generic;

namespace Stomatology3.Controllers.Users.UserViewModels
{
    public class UserRemovalModel
    {
        public IEnumerable<GetUsersModel> Users { get; set; }
        public string Message { get; set; }
    }
}
