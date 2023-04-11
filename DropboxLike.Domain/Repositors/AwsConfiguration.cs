using DropboxLike.Domain.Contracts;

namespace DropboxLike.Domain.Repositors;

public class AwsConfiguration : IAwsConfiguration
{
  private readonly IConfiguration _config;
  public AwsConfiguration()
  {
    BucketName = _config["AwsConfiguration:dropboxlike"];
    Region = _config["AwsConfiguration:Region"];
    AwsAccessKey = _config["AwsConfiguration:AwsAccessKey"];
    AwsSecretAccessKey = _config["AwsConfiguration:AwsSecretAccessKey"];
  }

  public string AwsAccessKey { get; set; }
  public string AwsSecretAccessKey { get; set; }
  public string BucketName { get; set; }
  public string Region { get; set; }
}