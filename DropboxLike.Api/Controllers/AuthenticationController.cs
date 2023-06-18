using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserEntity userEntity)
    {
        if (ModelState.IsValid)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);
            var user = new UserEntity
                { FirstName = userEntity.FirstName, LastName = userEntity.LastName, Email = userEntity.Email, Password = hashedPassword };

            var result = await _userService.RegisterUser(user);

            if (result.IsSuccessful)
            {
                return Ok();
            }

            return BadRequest(new { Message = result.FailureMessage });
        }

        return BadRequest(ModelState);
    }
}