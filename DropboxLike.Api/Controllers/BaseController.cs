using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

public class BaseController : ControllerBase
{
    protected ClaimsPrincipal GetClaims()
    {
        return HttpContext.User;
    }
    
    // TODO: Could be adding a method here to retrieve user ID from claims instead of returning claims.
}