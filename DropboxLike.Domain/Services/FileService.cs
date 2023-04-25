using DropboxLike.Domain.Models;
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

    public async Task<S3Response> UploadSingleFileAsync(IFormFile file)
    {
        return await _fileRepository.UploadFileAsync(file);
    }

    public async Task<FileObject> DownloadSingleFileAsync(string file)
    {
        return await _fileRepository.DownloadFileAsync(file);
    }
}