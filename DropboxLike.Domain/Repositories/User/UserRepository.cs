using DropboxLike.Domain.Data;
using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DropboxLike.Domain.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<OperationResult<object>> AddUser(UserEntity user)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<UserEntity>> GetUserById(string id)
    {
        throw new NotImplementedException();
    }
}