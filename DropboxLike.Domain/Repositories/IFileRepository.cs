using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Repositories;

public interface IFileRepository
{
  Task<S3Response> UploadFileAsync(IFormFile file);
  Task<byte[]> DownloadFileAsync(string fileId, string destinationFolderPath);
  Task<S3Response> DeleteFileAsync(string fileId);
}