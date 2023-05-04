using Microsoft.EntityFrameworkCore;

namespace DropboxLike.Domain.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }
  public DbSet<FileModel> FileModels { get; set; }
}