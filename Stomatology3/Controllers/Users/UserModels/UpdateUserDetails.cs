using System;
using System.Collections.Generic;

namespace Stomatology3.Controllers.Users.UserViewModels
{
    public class UpdateUserDetails
    {
        public string Id { get; set; }
        public string FirstName {get; set;}
        public string LastName { get; set; }
        public string Email { get; set;}
        public string Password { get; set;}
        //public IEnumerable<string> Roles { get; set; }
    }
}
