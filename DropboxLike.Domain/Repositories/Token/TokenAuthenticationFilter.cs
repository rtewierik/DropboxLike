using Amazon.Runtime.Credentials.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DropboxLike.Domain.Repositories.Token;

public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var tokenManager = context.HttpContext.RequestServices.GetService<ITokenManager>();

        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            context.Result = new UnauthorizedObjectResult("Authorization header not found");
            return;
        }

        var token = context.HttpContext.Request.Headers["Authorization"].ToString();

        try
        {
            var claimsPrincipal = tokenManager.VerifyToken(token);
        }
        catch (Exception ex)
        {
            context.ModelState.AddModelError("Unauthorized", ex.Message);
            context.Result = new UnauthorizedObjectResult(context.ModelState);
        }
    }
}