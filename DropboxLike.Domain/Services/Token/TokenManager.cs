using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DropboxLike.Domain.Services.Token;

public class TokenManager : ITokenManager
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly byte[] _secretKey;

    public TokenManager()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _secretKey = "rekfjdhabdjekkrnabrisnakelsntjsn"u8.ToArray();
    }
    
    public bool Authenticate(string email, string password)
    {
        if (!string.IsNullOrWhiteSpace(email) &&
            !string.IsNullOrWhiteSpace(password) &&
            email == "admin@email.com" && password == "password")
            return true;
        return false;
    }

    // TODO: Add an email or user ID argument to initialize a claim that allows us to identify the user from JWT.
    public string NewToken()
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Godfrey Owidi") }),
            Issuer = "localhost",
            Audience = "DropboxLike",
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var jwtString = _tokenHandler.WriteToken(token);
        return jwtString;
    }
}