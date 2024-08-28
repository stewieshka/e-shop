namespace Application.Common.Interfaces.Persistence.Repositories;

public interface IRepository<T>
{
    Task<int> CreateAsync(T entity);
    Task<T?> GetById(Guid id);
    Task<int> DeleteAsync(Guid id);
}