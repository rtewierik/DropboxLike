using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Implementations;

public interface IFileHandler
{
  Task<S3Response> UploadFileAsync(IFormFile file);
}