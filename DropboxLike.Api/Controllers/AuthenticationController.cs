using DropboxLike.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager;

    public AuthenticationController(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserEntity userEntity)
    {
        if (ModelState.IsValid)
        {
            var user = new UserEntity
                { FirstName = userEntity.FirstName, LastName = userEntity.LastName, Email = userEntity.Email };
            var result = await _userManager.CreateAsync(user, userEntity.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        return BadRequest(ModelState);
    }
}