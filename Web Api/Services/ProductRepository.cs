using Microsoft.EntityFrameworkCore;
using Web_Api.Interfaces;
using WebApi.Database;
using WebApi.Models;

namespace Web_Api.Services;
internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _appDbContext.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteProductAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _appDbContext.Products.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (product is not null)
        {
            _appDbContext.Products.Remove(product);
        }
    }

    public async Task<Product> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _appDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product is null)
            throw new ArgumentNullException($"{nameof(product)} is null");
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Products.Include(p => p.Category).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(long categoryId, CancellationToken cancellationToken)
    {
        return await _appDbContext.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken)
    {
        var query = _appDbContext.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId);
        foreach (var pair in filterParameters)
        {
            var key = pair.Key;
            var value = pair.Value;

            query = query.Where(p => p.Category.AdditionalFields.ContainsKey(key) && p.Category.AdditionalFields[key] == value);
        }
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public void UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _appDbContext.Products.Update(product);
    }
}
