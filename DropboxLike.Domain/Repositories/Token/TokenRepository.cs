using System.Collections.Concurrent;
using System.Security.Claims;
using System.Security.Cryptography;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Data.Entities;
using Microsoft.AspNetCore.Authentication;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenRepository : ITokeRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private ConcurrentDictionary<string, TokenInfo> _tokens = new ConcurrentDictionary<string, TokenInfo>(); 
    
    public TokenRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public bool Authenticate(string email, string password, out string token)
    {
        UserEntity user = ValidateCredentials(email, password);
        if (user != null)
        {
            token = Guid.NewGuid().ToString();
            AuthenticationTicket ticket = CreateAuthenticationTicket(email);
            _tokens.TryAdd(token, new TokenInfo(ticket));
            return true;
        }

        token = null;
        return false;
    }

    public AuthenticationTicket ValidateToken(string token)
    {
        TokenInfo tokenInfo = null;
        if (_tokens.TryGetValue(token, out tokenInfo))
        {
            return tokenInfo.Ticket;
        }

        return null;
    }

    private AuthenticationTicket CreateAuthenticationTicket(string user)
    {
        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user)
        };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, nameof(TokenAuthenticationHandler));
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        AuthenticationTicket authenticateTicket =
            new AuthenticationTicket(claimsPrincipal, TokenAuthenticationSchemeOptions.Name);
        return authenticateTicket;
    }

    private UserEntity ValidateCredentials(string email, string password)
    {
        SHA256 _cryptoProvider = SHA256CryptoServiceProvider.Create();
        MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(password));
        byte[] hashBytes = _cryptoProvider.ComputeHash(stream);
        string hashString = System.Convert.ToBase64String(hashBytes);

        List<UserEntity> users = _applicationDbContext.AppUsers.ToList();

        return users.FirstOrDefault(u => u.Email == email && u.Password == hashString) ?? throw new InvalidOperationException();
    }
}