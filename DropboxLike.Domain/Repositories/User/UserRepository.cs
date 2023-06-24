using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Data.Entities;
using DropboxLike.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DropboxLike.Domain.Repositories.User;

public class UserRepository : IUserRepository
{
        private readonly string _bucketName;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAmazonS3 _awsS3Client;
        public UserRepository(IOptions<AwsConfiguration> options, ApplicationDbContext applicationDbContext)
        {
            var configuration = options.Value;
            _bucketName = configuration.BucketName;
            _awsS3Client = new AmazonS3Client(configuration.AwsAccessKey, configuration.AwsSecretAccessKey, RegionEndpoint.GetBySystemName(configuration.Region));
            _applicationDbContext = applicationDbContext;
        }

  public async Task<OperationResult<object>> AddUser(UserEntity user)
  {
      //if (GetUserByEmail(user.Email) != null)
      //{
      //    return OperationResult<object>.Fail("User with the same email already exists.");
      //}
      user.Token = GenerateToken();
      
      await _applicationDbContext.AppUsers.AddAsync(user);
      await _applicationDbContext.SaveChangesAsync();

      var userId = user.Id;
      var folderKey = $"user_{userId}/";

      var putRequest = new PutObjectRequest
      {
          BucketName = _bucketName,
          Key = folderKey,
          ContentBody = string.Empty
      };

      using (var s3Client = new AmazonS3Client())
      {
          await s3Client.PutObjectAsync(putRequest);
      }

      return OperationResult<object>.Success(user);
  }

  public async Task<OperationResult<object>> AuthenticateUser(string email, string password)
  {
      var user = GetUserByEmail(email);
      if (user != null && VerifyPassword(password, user.Password))
      {
          var token = GetUserToken(user.Id);

          return OperationResult<object>.Success(token);
      }

      return null;
  }

  public string GenerateToken()
  {
      var tokenLength = 32;

      byte[] tokenBytes = new byte[tokenLength];

      using (var rng = new RNGCryptoServiceProvider())
      {
          rng.GetBytes(tokenBytes);
      }

      return Convert.ToBase64String(tokenBytes);
  }

  public bool VerifyPassword(string password, string hashedPassword)
  {
      var salt = Convert.FromBase64String(hashedPassword.Substring(0, 24));

      byte[] computedHash;
      using (var hmac = new HMACSHA256(salt))
      {
          computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }

      string computedHashBase64 = Convert.ToBase64String(computedHash);

      return hashedPassword.Equals(computedHashBase64);
  }
  
  public UserEntity GetUserByEmail(string email)
  {
      return _applicationDbContext.AppUsers.FirstOrDefault(x => x.Email == email);
     
  }
  public string GetUserToken(string userId)
  {
      var user = _applicationDbContext.AppUsers.FirstOrDefault(t => t.Id == userId);
        
      return user?.Token;
  }
}