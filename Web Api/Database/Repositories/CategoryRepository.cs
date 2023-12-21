using Microsoft.EntityFrameworkCore;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Entities;
using WebApi.Database;

namespace Web_Api.Database.Repositories;

/// <summary>
/// Репозиторий для работы с категориями.
/// </summary>
internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;
    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    /// <summary>
    /// Добавление категории в базу данных
    /// </summary>
    /// <param name="category">Категория</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task AddAsync(Category category, CancellationToken cancellationToken) =>
        await _appDbContext.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
    /// <summary>
    /// Удаление категории из базы данных
    /// </summary>
    /// <param name="id">ID категории</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} does not exist.");
        _appDbContext.Categories.Remove(category);
    }
    /// <summary>
    /// Получение списка категорий
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция категории</returns>
    public async Task<IEnumerable<Category>> GetCategoryAsync(CancellationToken cancellationToken) =>
        await _appDbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
    /// <summary>
    /// Получение категории по ID
    /// </summary>
    /// <param name="id">IDкатегории</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Категория</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} does not exist.");
        return category;
    }
    /// <summary>
    /// Обновление категории в базе данных
    /// </summary>
    /// <param name="category">Категория</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public void UpdateAsync(Category category, CancellationToken cancellationToken) =>
        _appDbContext.Categories.Update(category);
}
