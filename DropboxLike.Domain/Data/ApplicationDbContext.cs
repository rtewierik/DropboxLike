using DropboxLike.Domain.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DropboxLike.Domain.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
  public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
  }
  public DbSet<FileEntity> FileModels { get; set; }
  public DbSet<UserEntity> AppUsers { get; set;  }
}