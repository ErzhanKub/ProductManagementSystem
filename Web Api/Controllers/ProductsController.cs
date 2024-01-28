using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Contracts.ProductDto;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    public ProductsController(ProductService productService) => _productService = productService;

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
