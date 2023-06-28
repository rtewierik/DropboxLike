using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositories.File;

namespace DropboxLike.Domain.Services.File;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<OperationResult<object>> UploadSingleFileAsync(IFormFile file)
    {
        return await _fileRepository.UploadFileAsync(file);
    }

    public async Task<OperationResult<Models.File>> DownloadSingleFileAsync(string fileId)
    {
        return await _fileRepository.DownloadFileAsync(fileId);
    }

    public async Task<OperationResult<object>> DeleteSingleFileAsync(string fileId)
    {
        return await _fileRepository.DeleteFileAsync(fileId);
    }

    public async Task<OperationResult<List<FileEntity>>> ListBucketFilesAsync()
    {
        return await _fileRepository.ListFilesAsync();
    }
}