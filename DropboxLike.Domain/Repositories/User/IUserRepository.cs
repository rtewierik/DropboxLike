using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;

namespace DropboxLike.Domain.Repositories.User;

public interface IUserRepository
{
    Task<OperationResult<object>> AddUser(UserEntity user);
    Task<OperationResult<UserEntity>> GetUserById(string id);
}