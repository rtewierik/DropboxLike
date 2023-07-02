using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DropboxLike.Domain.Repositories.Token;

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

    public ClaimsPrincipal VerifyToken(string token)
    {
        var claims = _tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        }, out _);
        
        return claims;
    }
}