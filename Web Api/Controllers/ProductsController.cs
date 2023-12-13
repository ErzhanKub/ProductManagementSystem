using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.Contracts;
using Web_Api.Services;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduct(long id, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return response?.IsSuccess switch
        {
            true => Ok(response.Value),
            false => BadRequest($"Operation failed: {response.Reasons}"),
            null => throw new ArgumentNullException(nameof(response))
        };
    }

    [HttpGet("bycategory/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByCategory(long categoryId, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByCategoryAsync(categoryId, cancellationToken).ConfigureAwait(false);
        return response?.IsSuccess switch
        {
            true => Ok(response.Value),
            false => BadRequest($"Operation failed: {response.Reasons}"),
            null => throw new ArgumentNullException(nameof(response))
        };
    }

    [HttpGet("byfilter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByFilter([FromQuery] long categoryId, [FromQuery] Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByFilterAsync(categoryId, filter, cancellationToken).ConfigureAwait(false);
        return response?.IsSuccess switch
        {
            true => Ok(response.Value),
            false => BadRequest($"Operation failed: {response.Reasons}"),
            null => throw new ArgumentNullException(nameof(response))
        };
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct(ProductDto product, CancellationToken cancellationToken)
    {
        var response = await _productService.CreateProductAsync(product, cancellationToken).ConfigureAwait(false);
        return response?.IsSuccess switch
        {
            true => Created($"api/Products/{response.Value}", product),
            false => BadRequest($"Operation failed: {response.Reasons}"),
            null => throw new ArgumentNullException(nameof(response))
        };
    }
}
