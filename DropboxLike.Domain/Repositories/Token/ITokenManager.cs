using System.Security.Claims;

namespace DropboxLike.Domain.Repositories.Token;

public interface ITokenManager
{
    bool Authenticate(string email, string password);
    string NewToken();
    ClaimsPrincipal VerifyToken(string token);
}