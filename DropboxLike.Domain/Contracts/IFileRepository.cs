using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;

namespace DropboxLike.Domain.Contracts;
public interface IFileRepository
{
  Task<S3Response> UploadFileAsync(IFormFile file);
}