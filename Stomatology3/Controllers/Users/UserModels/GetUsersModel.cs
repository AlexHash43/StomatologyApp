using System.Collections.Generic;

namespace Stomatology3.Controllers.Users.UserViewModels
{
    public class GetUsersModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IList<string> Roles { get; set; }
    }
}
