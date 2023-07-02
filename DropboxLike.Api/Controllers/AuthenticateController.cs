using DropboxLike.Domain.Repositories.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DropboxLike.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly ITokenManager _tokenManager;

    public AuthenticateController(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }
    
    public IActionResult Authenticate(string email, string password)
    {
        if (_tokenManager.Authenticate(email, password))
        {
            return Ok(new { Token = _tokenManager.NewToken() });
        }
        ModelState.AddModelError("Unauthorized", "You are not authorized");
        return Unauthorized(ModelState);
    }
}