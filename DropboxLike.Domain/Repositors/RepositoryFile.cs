namespace DropboxLike.Domain.Repositors;

public class RepositoryFile : IRepository<File>
{
  private readonly ApplicationDbContext _appDbContext;
  private readonly ILogger _logger;
  public RepositoryFile(ILogger<File> logger)
  {
    _logger = logger;
  }
}