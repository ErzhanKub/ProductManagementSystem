using WebApi.Models;

namespace Web_Api.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
    Task<Category> GetCategoryByIdAsync(long id, CancellationToken cancellationToken);
    Task AddCategoryAsync(Category category, CancellationToken cancellationToken);
    Task DeleteCategoryAsync(long id, CancellationToken cancellationToken);
    void UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
}
