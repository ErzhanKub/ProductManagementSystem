using Microsoft.EntityFrameworkCore;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Entities;
using WebApi.Database;

namespace WebApi.Database.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken) =>
        await _appDbContext.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _appDbContext.Products.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (product is null)
            throw new KeyNotFoundException($"Рroduct with id {id} does not exist.");
        _appDbContext.Products.Remove(product);
    }

    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken) ??
        throw new KeyNotFoundException($"Product with id {id} does not exist.");

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken, long? categoryId = null) =>
        await _appDbContext.Products.Include(p => p.Category).AsNoTracking()
            .Where(p => categoryId == null || p.CategoryId == categoryId)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken)
    {
        // Получаем продукты только один раз
        var products = await _appDbContext.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken: cancellationToken);

        // Фильтруем продукты с помощью LINQ вместо цикла foreach
        return products.Where(p => filterParameters.All(fp => (p.AdditionalFields.ContainsKey(fp.Key) && p.AdditionalFields[fp.Key] == fp.Value) || (p.Category.AdditionalFields.ContainsKey(fp.Key) && p.Category.AdditionalFields[fp.Key] == fp.Value)));
    }

    public void UpdateAsync(Product product, CancellationToken cancellationToken) =>
        _appDbContext.Products.Update(product);
}
