using System.Collections.Generic;

namespace Stomatology3.Controllers.Auth.AuthModels
{
    public class RegisterUser
    {
        /// <summary>
        ///     Property for reading and writing the first name client.
        /// </summary>
        //public string FirstName { get; set; }
        /// <summary>
        ///     Property for reading and writing the last name from client.
        /// </summary>
        //public string LastName { get; set; }
        /// <summary>
        ///     Property for reading and writing the username from client.
        /// </summary>
        //public string UserName { get; set; }
        /// <summary>
        ///     Property for reading and writing the email from client.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///     Property for reading and writing the password from client.
        /// </summary>
        public string Password { get; set; }
        //public IEnumerable<string> Roles { get; set; }
    }
}
