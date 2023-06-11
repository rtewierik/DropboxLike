using DropboxLike.Domain.Models;
using DropboxLike.Domain.Repositories;
using File = DropboxLike.Domain.Models.File;

namespace DropboxLike.Domain.Services;

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

    public async Task<OperationResult<File>> DownloadSingleFileAsync(string fileId)
    {
        return await _fileRepository.DownloadFileAsync(fileId);
    }

    public async Task<OperationResult<object>> DeleteSingleFileAsync(string fileId)
    {
        return await _fileRepository.DeleteFileAsync(fileId);
    }

    public async Task<List<FileEntity>> ListBucketFilesAsync()
    {
        return await _fileRepository.ListFilesAsync();
    }
}