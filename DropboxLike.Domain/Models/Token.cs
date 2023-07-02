namespace DropboxLike.Domain.Models;

public class Token
{
    public string Value { get; set; }
    public DateTime ExpiryDate { get; set; }
}