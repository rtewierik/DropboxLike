using DropboxLike.Domain.Data.Entities;

namespace DropboxLike.Domain.Services.User;

public interface IUserService
{ 
    Task<UserEntity> GetUserByIdAsync(string id);
}