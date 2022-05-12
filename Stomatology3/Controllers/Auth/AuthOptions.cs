using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Stomatology3.Controllers.Auth
{
    public class AuthOptions
    {
        /// <summary>
        ///     JWT security token issuing party.
        /// </summary>
        public const string ISSUER = "PrivateServer";
        /// <summary>
        ///     JWT security token consumer.
        /// </summary>
        public const string AUDIENCE = "PrivateClient";
        const string KEY = "loremipsum";
        /// <summary>
        ///     JWT security token lifetime.
        /// </summary>
        public const int LIFETIME = 30;
        /// <summary>
        ///     JWT security token encoding method.
        /// </summary>
        /// <returns>   A new security key.</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
