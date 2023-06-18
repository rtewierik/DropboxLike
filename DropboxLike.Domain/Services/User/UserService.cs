using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositories.User;

namespace DropboxLike.Domain.Services.User;

public class UserService : IUserService
{
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<OperationResult<UserEntity>> GetUserByEmailAsync(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public Task<UserEntity> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
}