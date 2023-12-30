namespace WebApi.Abstractions.Shared;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    void UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}
