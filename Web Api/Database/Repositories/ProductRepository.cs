using Microsoft.EntityFrameworkCore;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Entities;

namespace WebApi.Database.Repositories;
/// <summary>
/// Репозиторий для работы с продуктами
/// </summary>
internal sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    /// <summary>
    /// Добавление продукта в базу данных
    /// </summary>
    /// <param name="product">Продукт</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task AddAsync(Product product, CancellationToken cancellationToken) =>
        await _appDbContext.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Удаление продукта из базы данных
    /// </summary>
    /// <param name="id">ID продукта</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _appDbContext.Products.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (product is null)
            throw new KeyNotFoundException($"Рroduct with id {id} does not exist.");
        _appDbContext.Products.Remove(product);
    }

    /// <summary>
    /// Получение продукта по ID
    /// </summary>
    /// <param name="id">ID продукта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Продукт</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Product> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken) ??
        throw new KeyNotFoundException($"Product with id {id} does not exist.");

    /// <summary>
    /// Получение списка продуктов с опциональным фильтром по ID категории
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <param name="categoryId">ID категории</param>
    /// <returns>Коллекция продуктов</returns>
    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken, long? categoryId = null) =>
        await _appDbContext.Products.Include(p => p.Category).AsNoTracking()
            .Where(p => categoryId == null || p.CategoryId == categoryId)
            .ToListAsync(cancellationToken: cancellationToken);

    /// <summary>
    /// Получение списка продуктов с фильтрацией по дополнительным полям
    /// </summary>
    /// <param name="categoryId">IDкатегории</param>
    /// <param name="filterParameters">Фильтры<key,value></param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция продуктов</returns>
    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(long categoryId, Dictionary<string, string> filterParameters, CancellationToken cancellationToken)
    {
        // Получаем продукты только один раз
        var products = await _appDbContext.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken: cancellationToken);

        // Фильтруем продукты с помощью LINQ вместо цикла foreach
        return products.Where(p => filterParameters.All(fp => (p.AdditionalFields.ContainsKey(fp.Key) && p.AdditionalFields[fp.Key] == fp.Value) || (p.Category.AdditionalFields.ContainsKey(fp.Key) && p.Category.AdditionalFields[fp.Key] == fp.Value)));
    }

    /// <summary>
    /// Обновление продукта в базе данных
    /// </summary>
    /// <param name="product">Продукт</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public void UpdateAsync(Product product, CancellationToken cancellationToken) =>
        _appDbContext.Products.Update(product);
}
