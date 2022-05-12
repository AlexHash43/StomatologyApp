using System.Collections.Generic;

namespace Stomatology3.Controllers.Auth.AuthModels
{
    public class ReturnUser
    {
        /// <summary>
        ///     Name property for ReturnUser.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Email property for ReturnUser.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///     UserName property for ReturnUser.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets and sets the user's roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
