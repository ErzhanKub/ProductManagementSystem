namespace Web_Api.Abstractions.Shared;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    void UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}
