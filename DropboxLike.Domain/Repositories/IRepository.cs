namespace DropboxLike.Domain.Repositories;

public interface IRepository<T>
{
  public Task<T> Create(T _object);
  public void Delete(T _object);
  public void Update(T _object);
  public IEnumerable<T> GetAll();
  public T GetById(int Id);
}