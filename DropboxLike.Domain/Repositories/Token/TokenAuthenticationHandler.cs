using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationSchemeOptions>
{
   private ITokeRepository _tokeRepository;
   private const string AUTHORIZATION_TOKEN_HEADER = "AuthToken";
   public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      ITokeRepository tokenRepository;
   ) : base(options, logger, encoder, clock)
   {
      _tokeRepository = tokenRepository;
   }

   protected override Task<AuthenticateResult> HandleAuthenticateAsync()
   {
      string authToken = this.Request.Headers[AUTHORIZATION_TOKEN_HEADER];
      if (string.IsNullOrEmpty(authToken))
      {
         return Task.FromResult(AuthenticateResult.Fail("Authorization header not found"));
      }

      AuthenticationTicket authTicket = _tokeRepository.ValidateToken(authToken);
      if (authTicket == null)
      {
         return Task.FromResult(AuthenticateResult.Fail(""));
      }

      return Task.FromResult(AuthenticateResult.Success(authTicket));
   }
   
}