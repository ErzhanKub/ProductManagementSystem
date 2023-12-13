using Microsoft.EntityFrameworkCore;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Entities;
using WebApi.Database;

namespace Web_Api.Database.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken) =>
        await _appDbContext.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _appDbContext.Products.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        _appDbContext.Products.Remove(product!);
    }

    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken) ??
        throw new ArgumentNullException($"product is null");

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken, long? categoryId = null) =>
        await _appDbContext.Products.Include(p => p.Category).AsNoTracking()
            .Where(p => categoryId == null || p.CategoryId == categoryId)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken)
    {
        var query = _appDbContext.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId);
        foreach (var pair in filterParameters)
        {
            var key = pair.Key;
            var value = pair.Value;

            query = query.Where(p => p.Category.AdditionalFields.Keys.Contains(key) && p.Category.AdditionalFields[key].ToString() == value);
        }
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public void UpdateAsync(Product product, CancellationToken cancellationToken) =>
        _appDbContext.Products.Update(product);
}
