using DropboxLike.Domain.Data;

namespace DropboxLike.Domain.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _applicationDbContext;

    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public Task<T> Create(T _object)
    {
        throw new NotImplementedException();
    }

    public void Delete(T _object)
    {
        throw new NotImplementedException();
    }

    public void Update(T _object)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T GetById(string id)
    {
        return _applicationDbContext.Set<T>().Find(id);
    }
}