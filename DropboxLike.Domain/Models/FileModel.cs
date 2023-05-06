
using System.ComponentModel.DataAnnotations;
public class FileModel
{
  [Key]
  public string? FileName { get; set; }
  public string? FileSize { get; set; }
  public string? ContentType { get; set; }
  public string? TimeStamp { get; set; }
}