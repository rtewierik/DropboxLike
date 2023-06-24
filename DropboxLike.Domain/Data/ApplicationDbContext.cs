using DropboxLike.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DropboxLike.Domain.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }
  public DbSet<FileEntity> FileModels { get; set; }
  public DbSet<UserEntity> AppUsers { get; set;  }
}