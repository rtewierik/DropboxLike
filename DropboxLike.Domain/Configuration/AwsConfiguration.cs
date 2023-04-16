namespace DropboxLike.Domain.Configuration;

public class AwsConfiguration
{
  public string AwsAccessKey { get; set; }
  public string AwsSecretAccessKey { get; set; }
  public string BucketName { get; set; }
  public string Region { get; set; }
}