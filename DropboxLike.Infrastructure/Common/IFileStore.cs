using DropboxLike.Domain.Data;

namespace DropboxLike.Domain.Contracts;

public interface IFileStore
{
  S3Response UploadSingleFile(IFormFile file);
}