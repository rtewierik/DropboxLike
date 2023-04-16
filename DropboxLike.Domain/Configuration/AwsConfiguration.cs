<<<<<<< Updated upstream
using DropboxLike.Domain.Configuration;

namespace DropboxLike.Domain.Repositors;

public class AwsConfiguration : IAwsConfiguration
{
  public AwsConfiguration()
  {
    BucketName = "AWS_BUCKET_NAME";
    AwsAccessKey = "AWS_ACCESS_KEY";
    AwsSecretAccessKey = "AWS_SECRET_ACCESS_KEY";
    Region = "AWS_REGION";
  }

=======
namespace DropboxLike.Domain.Configuration;

public class AwsConfiguration
{
>>>>>>> Stashed changes
  public string AwsAccessKey { get; set; }
  public string AwsSecretAccessKey { get; set; }
  public string BucketName { get; set; }
  public string Region { get; set; }
}