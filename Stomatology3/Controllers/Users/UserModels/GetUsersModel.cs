﻿using System.Collections.Generic;

namespace Stomatology3.Controllers.Users.UserViewModels
{
    public class GetUsersModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
