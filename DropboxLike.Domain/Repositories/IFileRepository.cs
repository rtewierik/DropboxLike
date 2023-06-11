using DropboxLike.Domain.Models;
using File = DropboxLike.Domain.Models.File;

namespace DropboxLike.Domain.Repositories;

public interface IFileRepository
{
  Task<OperationResult<object>> UploadFileAsync(IFormFile file);
  Task<OperationResult<File>> DownloadFileAsync(string fileId);
  Task<OperationResult<object>> DeleteFileAsync(string fileId);
  Task<List<FileEntity>> ListFilesAsync();
}