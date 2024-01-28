using WebApi.Abstractions.Shared;
using WebApi.Models.Entities;

namespace WebApi.Abstractions.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken, long? categoryId = null);
    Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken);
}
