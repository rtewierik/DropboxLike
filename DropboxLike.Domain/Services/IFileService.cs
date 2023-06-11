using DropboxLike.Domain.Models;
using File = DropboxLike.Domain.Models.File;

namespace DropboxLike.Domain.Services;

public interface IFileService
{
    Task<OperationResult<object>> UploadSingleFileAsync(IFormFile file);
    Task<OperationResult<File>> DownloadSingleFileAsync(string fileId);
    Task<OperationResult<object>> DeleteSingleFileAsync(string fileId);
    Task<List<FileEntity>> ListBucketFilesAsync();
}