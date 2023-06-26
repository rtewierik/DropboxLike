using Microsoft.AspNetCore.Authentication;

namespace DropboxLike.Domain.Repositories.Token;

public interface ITokeRepository
{
    bool Authenticate(string user, string password, out string token);
    AuthenticationTicket ValidateToken(string token);
}