using System.Globalization;
using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DropboxLike.Domain.Configuration;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using File = DropboxLike.Domain.Models.File;

namespace DropboxLike.Domain.Repositories;

public class FileRepository : IFileRepository
{
  private readonly string _bucketName;
  private readonly ApplicationDbContext _applicationDbContext;
  private readonly IAmazonS3 _awsS3Client;
  private readonly IMemoryCache _memoryCache;
  public FileRepository(IOptions<AwsConfiguration> options, ApplicationDbContext applicationDbContext, IMemoryCache memoryCache)
  {
    var configuration = options.Value;
    _bucketName = configuration.BucketName;
    _awsS3Client = new AmazonS3Client(configuration.AwsAccessKey, configuration.AwsSecretAccessKey, RegionEndpoint.GetBySystemName(configuration.Region));
    _applicationDbContext = applicationDbContext;
    _memoryCache = memoryCache;

  }

  public async Task<OperationResult<object>> UploadFileAsync(IFormFile file)
  {
    try
    {
      using (var newMemoryStream = new MemoryStream())
      {
        await file.CopyToAsync(newMemoryStream);
        var uploadRequest = new TransferUtilityUploadRequest
        {
          InputStream = newMemoryStream,
          Key = $"{Guid.NewGuid()}",
          BucketName = _bucketName,
          ContentType = file.ContentType,
          CannedACL = S3CannedACL.NoACL
        };
        var transferUtility = new TransferUtility(_awsS3Client);

        await transferUtility.UploadAsync(uploadRequest);

        var fileModel = new FileEntity
        {
          FileKey = uploadRequest.Key,
          FileName = file.FileName,
          FileSize = file.Length.ToString(),
          ContentType = file.ContentType,
          TimeStamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        };

        _applicationDbContext.FileModels.Add(fileModel);
        await _applicationDbContext.SaveChangesAsync();
        
        return OperationResult<object>.Success(new object(), HttpStatusCode.Created);
      }
    }
    catch (AmazonS3Exception exception)
    {
      var message = $"{exception.StatusCode}: {exception.Message}";
      return OperationResult<object>.Fail(exception, message, exception.StatusCode);
    }
    catch (Exception exception)
    {
      var message = $"{HttpStatusCode.InternalServerError}: {exception.Message}";
      return OperationResult<object>.Fail(exception, message);
    }
  }

  public async Task<OperationResult<File>> DownloadFileAsync(string fileId)
  {
    try
    {
      var file = await _applicationDbContext.FileModels.FindAsync(fileId);
      if (file == null)
      {
        throw new FileNotFoundException("File not Found");
      }

      var listRequest = new ListObjectsV2Request
      {
        BucketName = _bucketName,
      };
      if (_memoryCache.TryGetValue<List<string>>(_bucketName, out var fileNames))
      {
        var fileCache = new File
        {
          ObjectNames = fileNames
        };
        return OperationResult<File>.Success(fileCache);
      }
      fileNames = new List<string>();
      
      ListObjectsV2Response listResponse;
      do
      {
        listResponse =  await _awsS3Client.ListObjectsV2Async(listRequest);
        
        fileNames.AddRange(listResponse.S3Objects.Select(obj => obj.Key));

        listRequest.ContinuationToken = listResponse.NextContinuationToken;
      } while (listResponse.IsTruncated);

      var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));

      _memoryCache.Set(_bucketName, fileNames, cacheOptions);

      foreach (var obj in fileNames)
      {
        if (file?.FileKey == obj)
        {
          var downloadFileName = file.FileName;
          var filePath = Path.Combine("/home/godfreyowidi/Downloads/DropboxLike", downloadFileName);
          
          // Check if a file with the same name already exists
          if (System.IO.File.Exists(filePath))
          {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(downloadFileName);
            var fileExtension = Path.GetExtension(downloadFileName);

            string newFileName;
            string newFilePath;

            int suffixCounter = 1;

            do
            {
              newFileName = $"{fileNameWithoutExtension}({suffixCounter}){fileExtension}";
              newFilePath = Path.Combine("/home/godfreyowidi/Downloads/DropboxLike", newFileName);
              
              // Increment the counter
              suffixCounter++;
            } while (System.IO.File.Exists(newFilePath));

            filePath = newFilePath;
          }
          GetObjectRequest request = new GetObjectRequest
          {
            BucketName = _bucketName,
            Key = WebUtility.HtmlDecode(fileId).ToLowerInvariant()
          };
          using var response = await _awsS3Client.GetObjectAsync(request);
          {

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
              await response.ResponseStream.CopyToAsync(fileStream);
            }
            var contentType = response.Headers.ContentType;
            return OperationResult<File>.Success(new File
            {
              FileStream = response.ResponseStream,
              ContentType = contentType
            });
          }
        }
      }

      return OperationResult<File>.Fail($"{HttpStatusCode.NotFound}: File not found.", HttpStatusCode.NotFound);
    }
    catch (AmazonS3Exception exception)
    {
      var message = $"{exception.StatusCode}: {exception.Message}";
      return OperationResult<File>.Fail(exception, message, exception.StatusCode);
    }
    catch (Exception exception)
    {
      var message = $"{HttpStatusCode.InternalServerError}: {exception.Message}";
      return OperationResult<File>.Fail(exception, message);
    }
  }

  public async Task<OperationResult<object>> DeleteFileAsync(string fileId)
  {
    var results = await _applicationDbContext.FileModels
        .FirstOrDefaultAsync(x => x.FileKey == fileId);

    var listRequest = new ListObjectsV2Request
    {
      BucketName = _bucketName,
    };
    var listResponse = await _awsS3Client.ListObjectsV2Async(listRequest);

    foreach (var obj in listResponse.S3Objects)
    {
      if (results?.FileKey != obj.Key) continue;
      var request = new DeleteObjectRequest
      {
        BucketName = _bucketName,
        Key = WebUtility.HtmlDecode(fileId).ToLowerInvariant()
      };

      await _awsS3Client.DeleteObjectAsync(request);

      _applicationDbContext.FileModels.Remove(results);
      await _applicationDbContext.SaveChangesAsync();
      
      return OperationResult<object>.Success(new object(), HttpStatusCode.NoContent);
    }
    return OperationResult<object>.Fail($"{HttpStatusCode.NotFound}: File not found.", HttpStatusCode.NotFound);
  }
}