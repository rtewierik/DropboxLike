using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Models.Response;
using Microsoft.Extensions.Options;

namespace DropboxLike.Domain.Implementations;

public class FileHandler : IFileHandler
{
  private readonly string _bucketName;
  private readonly IAmazonS3 _awsS3Client;

  private S3Response _response = new();

  public FileHandler(IOptions<AwsConfiguration> options)
  {
    var configuration = options.Value;
    _bucketName = configuration.BucketName;
    _awsS3Client = new AmazonS3Client(configuration.AwsAccessKey, configuration.AwsSecretAccessKey, RegionEndpoint.GetBySystemName(configuration.Region));
  }

  public async Task<S3Response> UploadFileAsync(IFormFile file)
  {
    try
    {
      using (var newMemoryStream = new MemoryStream())
      {
        file.CopyTo(newMemoryStream);
        var uploadRequest = new TransferUtilityUploadRequest()
        {
          InputStream = newMemoryStream,
          Key = file.Name,
          BucketName = _bucketName,
          ContentType = file.ContentType,
          CannedACL = S3CannedACL.NoACL 
        };
        var transferUtility = new TransferUtility(_awsS3Client);
  
        await transferUtility.UploadAsync(uploadRequest);

        _response.StatusCode = 200;
        _response.Message = $"{file.Name} has been uploaded to s3 successfully";
      }
    }
    catch (AmazonS3Exception ex)
    {
      _response.StatusCode = (int)ex.StatusCode;
      _response.Message = ex.Message;
    }
    catch (Exception ex)
    {
      _response.StatusCode = 500;
      _response.Message = ex.Message;
    }
    return _response;
  }
}