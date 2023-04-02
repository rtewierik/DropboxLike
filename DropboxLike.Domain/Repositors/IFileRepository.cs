using DropboxLike.Domain.Contracts;
using File = DropboxLike.Domain.Models.File;

namespace DropboxLike.Domain.Repositors;

public interface IFileRepository : IRepository<File>
{
}