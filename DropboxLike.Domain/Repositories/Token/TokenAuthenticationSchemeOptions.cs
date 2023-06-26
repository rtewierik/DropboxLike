using Microsoft.AspNetCore.Authentication;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "TokenAuthenticationScheme";
}