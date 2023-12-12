using Microsoft.AspNetCore.Mvc;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Contracts;
using Web_Api.Models.Entities;
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
            var response = await _categoryService.GetCategoriesAsync(cancellationToken);
            if (response.IsSuccess)
                return Ok(response.Value);
            return BadRequest($"Operation failed: {response.Reasons}");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category, CancellationToken cancellationToken)
        {
            var response = await _categoryService.CreateCategoryAsync(category, cancellationToken);
            if (response.IsSuccess)
                return Ok("Category created successfully");
            return BadRequest($"Operation failed: {response.Reasons}");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            if (response.IsSuccess)
                return Ok("Category deleted successfully");
            return BadRequest($"Operation failed: {response.Reasons}");
        }
    }
}
