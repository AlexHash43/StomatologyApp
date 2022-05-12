using Stomatology3.Controllers.Auth.AuthModels;

namespace Stomatology3.Interfaces
{
    public interface IJwtHandlerAuth
    {
        string Authentication(AuthUser authUser, string role);
    }
}
