using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DropboxLike.Domain.Repositories;

public interface IFileRepository
{
  Task<S3Response> UploadFileAsync(IFormFile file);
  Task<FileStreamResult> DownloadFileAsync(string fileId);
  Task<S3Response> DeleteFileAsync(string fileId);
}