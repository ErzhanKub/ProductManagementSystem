using Web_Api.Abstractions.Shared;
using Web_Api.Models.Entities;

namespace Web_Api.Abstractions.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(long categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken);
}
