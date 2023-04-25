using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Models;
using DropboxLike.Domain.Models.Response;
using Microsoft.Extensions.Options;

namespace DropboxLike.Domain.Repositories;

public class FileRepository : IFileRepository
{
  private readonly string _bucketName;
  private readonly IAmazonS3 _awsS3Client;

  private S3Response _response = new();

  public FileRepository(IOptions<AwsConfiguration> options)
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

  public async Task<FileObject> DownloadFileAsync(string uniqueStorageName)
  {

    GetObjectRequest request = new GetObjectRequest
    {
      BucketName = _bucketName,
      Key = WebUtility.HtmlDecode(uniqueStorageName).ToLowerInvariant()
    };
    using var response = await _awsS3Client.GetObjectAsync(request);
    await using var responseStream = response.ResponseStream;
    await using var memory = new MemoryStream();

    var originalFileName = response.Metadata["x-amz-meta-tile"];
    var contentType = response.Metadata["x-amz-meta-content-type"];
    await responseStream.CopyToAsync(memory);
    var responseBody = memory.ToArray();

    return new FileObject(
      originalFileName,
      uniqueStorageName,
      contentType,
      responseBody,
      response.LastModified
    );
  }
}