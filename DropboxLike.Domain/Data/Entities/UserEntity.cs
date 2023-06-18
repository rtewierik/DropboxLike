using Microsoft.AspNetCore.Identity;

namespace DropboxLike.Domain.Data.Entities;
public class UserEntity : IdentityUser
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}