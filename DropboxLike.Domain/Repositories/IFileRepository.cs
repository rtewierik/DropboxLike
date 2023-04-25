using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Repositories;

public interface IFileRepository
{
  Task<S3Response> UploadFileAsync(IFormFile file);
  Task<FileObject> DownloadFileAsync(string fileId);
}