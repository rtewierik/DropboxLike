using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Services;

public interface IFileService
{
    Task<S3Response> UploadSingleFileAsync(IFormFile file);
    Task<byte[]> DownloadSingleFileAsync(string fileId, string destinationFolderPath);
}