using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.Contracts.ProductDto;
using Web_Api.Services;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    public ProductsController(ProductService productService) => _productService = productService;

    /// <summary>
    /// Получить продукт
    /// </summary>
    /// <param name="id">ID продукта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduct([FromRoute] long id, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductByIdAsync(id, cancellationToken);
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
    /// Получить продукты по категории
    /// </summary>
    /// <param name="id">ID категории</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("bycategory/{id}")]
    [ProducesResponseType(typeof(List<ProductGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByCategory([FromRoute] long id, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByCategoryAsync(id, cancellationToken);
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
    /// Получить продукты по фильтрам
    /// </summary>
    /// <param name="id">ID категории</param>
    /// <param name="filter">Фильтр <key, value></param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet("byfilter/{id}")]
    [ProducesResponseType(typeof(List<ProductGetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByFilter([FromRoute] long id, [FromQuery] Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByFilterAsync(id, filter, cancellationToken);
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
    /// Создать продукт
    /// </summary>
    /// <param name="product">Продукт</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost]
    [ProducesResponseType(typeof(ProductGetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct(ProductPostDto product, CancellationToken cancellationToken)
    {
        var response = await _productService.CreateProductAsync(product, cancellationToken);
        return response.IsSuccess switch
        {
            true => Created($"api/Products/{response.Value.Id}", response.Value),
            false => BadRequest(new ProblemDetails
            {
                Title = "Operation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = response.Reasons.First().Message,
            })
        };
    }
}
