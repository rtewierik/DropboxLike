using DropboxLike.Domain.Contracts;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Repositors;

namespace DropboxLike.Infrastructure.Common;

public class FileStore : IFileStore
{
  private readonly IFileRepository _fileRepository;

  public FileStore(AwsConfiguration awsConfiguration)
  {
    _fileRepository = new FileRepository(awsConfiguration.AwsAccessKey, awsConfiguration.AwsSecretAccessKey, awsConfiguration.Region, awsConfiguration.BucketName);
  }

  public S3Response UploadSingleFile(IFormFile file)
  {
    return _fileRepository.UploadFileAsync(file).Result;
  }
}