using Microsoft.AspNetCore.Authentication;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenInfo
{
    public TokenInfo() {}
    public DateTime CreatedAt { get; set; }
    public AuthenticationTicket Ticket { get; set; }
    public TokenInfo(AuthenticationTicket ticket)
    {
        CreatedAt = DateTime.Now;
        Ticket = ticket;
    }
}