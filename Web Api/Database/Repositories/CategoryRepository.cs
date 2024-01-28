using Microsoft.EntityFrameworkCore;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Entities;

namespace WebApi.Database.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;
    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task AddAsync(Category category, CancellationToken cancellationToken) =>
        await _appDbContext.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
    
    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.FindAsync(new object?[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} does not exist.");
        _appDbContext.Categories.Remove(category);
    }
    
    public async Task<IEnumerable<Category>> GetCategoryAsync(CancellationToken cancellationToken) =>
        await _appDbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
    
    public async Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} does not exist.");
        return category;
    }

    public void UpdateAsync(Category category, CancellationToken cancellationToken) =>
        _appDbContext.Categories.Update(category);
}
