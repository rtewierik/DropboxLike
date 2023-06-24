using DropboxLike.Domain.Data;
using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DropboxLike.Domain.Repositories.User;

public class Token
{
    private readonly ApplicationDbContext _applicationDbContext;

    public Token(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public void SaveToken(string userId, string token)
    {
        UpdateUserToken(userId, token);
    }
    
    public bool ValidateToken(UserEntity userId, string token)
    {
        var storedToken = GetUserToken(userId);

        return token == storedToken;
    }

    public void InvalidateToken(string token)
    {
        ClearUserToken(token);
    }

    public void UpdateUserToken(string userId, string token)
    {
        using (var dbContext = _applicationDbContext)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.Token = token;
                dbContext.SaveChanges();
            }
        }
    }
    
    public string GetUserToken(UserEntity userId)
    {
        var token = _applicationDbContext.AppUsers.FirstOrDefault(userId);
        
        return token.ToString()!;
    }
    
    public void ClearUserToken(string token)
    {
        using (var dbContext = _applicationDbContext)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Token == token);

            if (user != null)
            {
                user.Token = null;
                dbContext.SaveChanges();
            }
        }
    }
}