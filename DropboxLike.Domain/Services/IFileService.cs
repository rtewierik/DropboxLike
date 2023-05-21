using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Domain.Services;

public interface IFileService
{
    Task<S3Response> UploadSingleFileAsync(IFormFile file);
    Task<FileStreamResult> DownloadSingleFileAsync(string fileId);
    Task<S3Response> DeleteSingleFileAsync(string fileId);
}