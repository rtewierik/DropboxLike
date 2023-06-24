using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;

namespace DropboxLike.Domain.Services.User;

public interface IUserService
{ 
    Task<OperationResult<object>> RegisterUser(UserEntity user);
    Task<OperationResult<object>> LoginUser(string email, string password);
}