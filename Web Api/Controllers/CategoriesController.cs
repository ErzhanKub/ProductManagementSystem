using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.Contracts.CategoryDto;
using Web_Api.Services;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService) => _categoryService = categoryService;

        /// <summary>
        /// Получить список категории
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Создать категорию
        /// </summary>
        /// <param name="category">Категория dto</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="id">ID категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
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
