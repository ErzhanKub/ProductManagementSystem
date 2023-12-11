using Microsoft.EntityFrameworkCore;
using Web_Api.Interfaces;
using WebApi.Database;
using WebApi.Models;

namespace Web_Api.Services;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _appDbContext.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteCategoryAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is not null)
        {
            _appDbContext.Categories.Remove(category);
        }
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Categories.ToListAsync(cancellationToken);
    }

    public async Task<Category> GetCategoryByIdAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new ArgumentNullException(nameof(category));
        return category;
    }

    public void UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        _appDbContext.Categories.Update(category);
    }
}
