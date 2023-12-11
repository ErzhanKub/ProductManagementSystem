using WebApi.Models;

namespace Web_Api.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
    Task<Product> GetProductByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(long categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken);
    Task AddProductAsync(Product product, CancellationToken cancellationToken);
    void UpdateProductAsync(Product product, CancellationToken cancellationToken);
    Task DeleteProductAsync(long id, CancellationToken cancellationToken);
}
