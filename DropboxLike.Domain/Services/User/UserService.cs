using Amazon.Runtime.Internal.Auth;
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
        
        public async Task<OperationResult<object>> RegisterUser(UserEntity user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<OperationResult<object>> LoginUser(string email, string password)
        {
            return await _userRepository.AuthenticateUser(email, password);
        }
}