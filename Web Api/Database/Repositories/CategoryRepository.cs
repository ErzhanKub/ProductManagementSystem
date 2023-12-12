using Microsoft.EntityFrameworkCore;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Entities;
using WebApi.Database;

namespace Web_Api.Database.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _appDbContext.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new ArgumentNullException(nameof(category));
        _appDbContext.Categories.Remove(category);
    }

    public async Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new ArgumentNullException(nameof(category));
        return category;
    }

    public void UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        _appDbContext.Categories.Update(category);
    }
}