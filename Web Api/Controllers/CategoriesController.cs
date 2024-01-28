using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Contracts.CategoryDto;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService) => _categoryService = categoryService;

        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var response = await _categoryService.GetCategoriesAsync(cancellationToken);
            return response.IsSuccess switch
            {
                true => Ok(response.Value),
                false => BadRequest(new ProblemDetails
                {
                    Title = "Operation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = response.Reasons.First().Message,
                })
            };
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryGetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryPostDto category, CancellationToken cancellationToken)
        {
            var response = await _categoryService.CreateCategoryAsync(category, cancellationToken);
            return response.IsSuccess switch
            {
                true => Created($"api/Categories/{response.Value.Id}", response.Value),
                false => BadRequest(new ProblemDetails
                {
                    Title = "Operation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = response.Reasons.First().Message,
                })
            };
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id, CancellationToken cancellationToken)
        {
            var response = await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            return response.IsSuccess switch
            {
                true => NoContent(),
                false => BadRequest(new ProblemDetails
                {
                    Title = "Operation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = response.Reasons.First().Message,
                })
            };
        }
    }
}
