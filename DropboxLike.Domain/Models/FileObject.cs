namespace DropboxLike.Domain.Models;

public class FileObject
{
  public string? Name { get; set; } = null!;
  public MemoryStream? InputStream { get; set; } = null!;
  public string? BucketName { get; set; }
}