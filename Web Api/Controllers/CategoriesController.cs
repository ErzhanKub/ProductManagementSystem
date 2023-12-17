using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.Contracts;
using Web_Api.Services;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService) => _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetCategoriesAsync(cancellationToken).ConfigureAwait(false);
            return response?.IsSuccess switch
            {
                true => Ok(response.Value),
                false => BadRequest($"Operation failed: {response.Reasons}"),
                null => throw new ArgumentNullException(nameof(response)),
            };
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category, CancellationToken cancellationToken)
        {
            var response = await _categoryService.CreateCategoryAsync(category, cancellationToken).ConfigureAwait(false);
            return response?.IsSuccess switch
            {
                true => Created($"api/Categories/{response.Value}", category),
                false => BadRequest($"Operation failed: {response.Reasons}"),
                null => throw new ArgumentNullException(nameof(response)),
            };
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.DeleteCategoryAsync(id, cancellationToken).ConfigureAwait(false);
            return response?.IsSuccess switch
            {
                true => NoContent(),
                false => BadRequest($"Operation failed: {response.Errors}"),
                null => throw new ArgumentNullException(nameof(response)),
            };
        }
    }
}
