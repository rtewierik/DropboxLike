using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenManager : ITokenManager
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private byte[] secretKey;

    public TokenManager()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        secretKey = Encoding.ASCII.GetBytes("rekfjdhabdjekkrnabrisnakelsntjsn");
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
            Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "Godfrey Owidi") }),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
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
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);
        
        return claims;
    }
}