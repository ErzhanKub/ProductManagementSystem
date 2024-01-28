using WebApi.Abstractions.Shared;
using WebApi.Models.Entities;

namespace WebApi.Abstractions.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoryAsync(CancellationToken cancellationToken);
}
