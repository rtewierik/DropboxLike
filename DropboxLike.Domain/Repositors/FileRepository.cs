using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using DropboxLike.Domain.Contracts;
using DropboxLike.Domain.Data;
using DropboxLike.Domain.Models;

namespace DropboxLike.Domain.Repositors;

public class FileRepository : IFileRepository
{
  public async Task<S3Response> UploadFileAsync(FileObject file, AwsCredentials aWSCredentials)
  {
    // Adding AWS Creds
    var credentials = new BasicAWSCredentials(aWSCredentials.AwsKey, aWSCredentials.AwsSecretKey);

    // Region
    var config = new AmazonS3Config() { RegionEndpoint = Amazon.RegionEndpoint.USEast1};

    var response = new S3Response();

    try
    {
      // Payload
      var uploadRequest = new TransferUtilityUploadRequest()
      {
        InputStream = file.InputStream,
        Key = file.Name,
        BucketName = file.BucketName,
        CannedACL = S3CannedACL.NoACL 
      };

      // Create S3 client
      using var client = new AmazonS3Client(credentials, config);

      // Upload utility to S3
      var transferUtility = new TransferUtility(client);

      // Upload
      await transferUtility.UploadAsync(uploadRequest);

      response.StatusCode = 200;
      response.Message = $"{file.Name} has been uploaded to s3 successfully";
    }
    catch (AmazonS3Exception ex)
    {
      response.StatusCode = (int)ex.StatusCode;
      response.Message = ex.Message;
    }
    catch (Exception ex)
    {
      response.StatusCode = 500;
      response.Message = ex.Message;
    }
    return response;
  }
}