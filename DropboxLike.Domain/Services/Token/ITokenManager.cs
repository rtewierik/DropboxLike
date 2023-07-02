namespace DropboxLike.Domain.Services.Token;

public interface ITokenManager
{
    bool Authenticate(string email, string password);
    string NewToken();
}