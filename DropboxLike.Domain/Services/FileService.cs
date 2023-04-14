using DropboxLike.Domain.Models.Response;

namespace DropboxLike.Domain.Services;

public class FileService : IFileService
{
    private readonly IFileService _fileService;

    public FileService(IFileService fileService)
    {
        _fileService = fileService;
    }

  public S3Response UploadSingleFile(IFormFile file)
  {
    throw new NotImplementedException();
  }
}