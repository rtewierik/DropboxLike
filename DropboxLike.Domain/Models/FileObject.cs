namespace DropboxLike.Domain.Models;

public record FileObject
{
  public FileObject(
    string originalFileName,
    string uniqueStorageName,
    string contentType,
    byte[] content,
    DateTimeOffset lastModifiedAt
  )
  {
    Content = content;
    OriginalFileName = originalFileName;
    UniqueStorageName = uniqueStorageName;
    LastModifiedAt = lastModifiedAt;
    ContentType = contentType;
  }

  public byte[] Content { get; }
  public string OriginalFileName { get; }
  public string UniqueStorageName { get; }
  public DateTimeOffset LastModifiedAt { get; }
  public string ContentType { get; }
}