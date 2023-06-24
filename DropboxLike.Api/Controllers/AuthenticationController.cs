using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime.Internal.Util;
using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;
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
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(userEntity.Password);
                var hashedBytes = sha256.ComputeHash(passwordBytes);
                var hashedPassword = Convert.ToBase64String(hashedBytes);
                var user = new UserEntity
                    { Email = userEntity.Email, Token = userEntity.Token, Password = hashedPassword };

                var result = await _userService.RegisterUser(user);

                if (result.IsSuccessful)
                {
                    return Ok();
                }

                return BadRequest(new { Message = result.FailureMessage });
            }
        }

        return BadRequest(ModelState);
    }
    
    [HttpPost("login")]
    public async Task<OperationResult<object>> LoginUserAsync(LoginRequest request)
    {
        var token = await _userService.LoginUser(request.Email, request.Password);

        if (token != null)
        {
            return OperationResult<object>.Success(token);
        }

        return OperationResult<object>.Fail("Invalid Credentials");
    }
}