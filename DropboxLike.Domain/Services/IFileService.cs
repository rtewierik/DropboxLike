using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Services;

public interface IFileService
{
    S3Response UploadSingleFile(IFormFile file);
}