using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;
using Microsoft.Extensions.Options;

namespace DropboxLike.Domain.Repositories;

public class FileRepository : IFileRepository
{
  private readonly string _bucketName;
  private readonly ApplicationDbContext _applicationDbContext;
  private readonly IAmazonS3 _awsS3Client;

  private S3Response _response = new();

  public FileRepository(IOptions<AwsConfiguration> options, ApplicationDbContext applicationDbContext)
  {
    var configuration = options.Value;
    _bucketName = configuration.BucketName;
    _awsS3Client = new AmazonS3Client(configuration.AwsAccessKey, configuration.AwsSecretAccessKey, RegionEndpoint.GetBySystemName(configuration.Region));
    _applicationDbContext = applicationDbContext;
  }

  public async Task<S3Response> UploadFileAsync(IFormFile file)
  {
    try
    {
      using (var newMemoryStream = new MemoryStream())
      {
        file.CopyTo(newMemoryStream);
        var fileExt = Path.GetExtension(file.Name);
        var uploadRequest = new TransferUtilityUploadRequest()
        {
          InputStream = newMemoryStream,
          Key = $"{Guid.NewGuid()}.{fileExt}",
          BucketName = _bucketName,
          ContentType = file.ContentType,
          CannedACL = S3CannedACL.NoACL
        };
        var transferUtility = new TransferUtility(_awsS3Client);

        await transferUtility.UploadAsync(uploadRequest);

        var fileModel = new FileModel
        {
          FileName = uploadRequest.Key,
          FileSize = (file.Length).ToString(),
          ContentType = file.ContentType,
          TimeStamp = (DateTime.UtcNow).ToString()
        };

        _applicationDbContext.FileModels.Add(fileModel);
        await _applicationDbContext.SaveChangesAsync();

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

  public async Task<byte[]> DownloadFileAsync(string fileId, string destinationFolderPath)
  {
    GetObjectRequest request = new GetObjectRequest
    {
      BucketName = _bucketName,
      Key = WebUtility.HtmlDecode(fileId).ToLowerInvariant()
    };
    using var response = await _awsS3Client.GetObjectAsync(request);

    var filePath = Path.Combine(destinationFolderPath, fileId);

    using (var fileStream = File.Create(filePath))
    {
      await response.ResponseStream.CopyToAsync(fileStream);
    }

    byte[] result = System.Text.Encoding.UTF8.GetBytes(filePath);
    return result;
  }

  public async Task<S3Response> DeleteFileAsync(string fileId)
  {
    DeleteObjectRequest request = new DeleteObjectRequest
    {
      BucketName = _bucketName,
      Key = WebUtility.HtmlDecode(fileId).ToLowerInvariant()
    };

    var response = await _awsS3Client.DeleteObjectAsync(request);

    _response.StatusCode = 204;
    _response.Message = $"{fileId} has been deleted successfully.";

    return _response;
  }
}