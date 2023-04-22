using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Services;

public interface IFileService
{
    Task<S3Response> UploadSingleFileAsync(IFormFile file);
}