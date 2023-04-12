using DropboxLike.Domain.Models.Response;
using DropboxLike.Domain.Repositories;

namespace DropboxLike.Domain.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public S3Response UploadSingleFile(IFormFile file)
    {
        return _fileRepository.UploadFileAsync(file).Result;
    }
}