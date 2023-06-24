using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DropboxLike.Domain.Models;

public class LoginRequest
{
    public string? Email { get; set;  }
    public string? Password { get; set;  }
}