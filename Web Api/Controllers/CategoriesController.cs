using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Interfaces;
using WebApi.Models;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository categoryRepository, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesAsync(cancellationToken);
                _logger.LogInformation("Categories successfully received");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the category");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryRepository.AddCategoryAsync(category, cancellationToken);
                _logger.LogInformation("Created category: {value}", category.Id);
                return Created($"api/Categories/{category.Id}", category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the category");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] Category category, CancellationToken cancellationToken)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest();

                _categoryRepository.UpdateCategoryAsync(category, cancellationToken);
                _logger.LogInformation("Updated category: {category.Id}", category.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the category");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id, CancellationToken cancellationToken)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id, cancellationToken);
                _logger.LogInformation("Deleted category: {value}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category");
                throw;
            }
        }
    }
}
