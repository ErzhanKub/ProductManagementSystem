using Microsoft.AspNetCore.Mvc;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Contracts;
using Web_Api.Models.Entities;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduct(long id, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductByIdAsync(id, cancellationToken);
        if (response.IsSuccess)
            return Ok(response.Value);
        return BadRequest(response.Reasons);
    }

    [HttpGet("bycategory/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByCategory(long categoryId, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByCategoryAsync(categoryId, cancellationToken);
        if (response.IsSuccess) return Ok(response.Value);
        return BadRequest(response.Reasons);
    }

    [HttpGet("byfilter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsByFilter([FromQuery] long categoryId, [FromQuery] Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductsByFilterAsync(categoryId, filter, cancellationToken);
        if (response.IsSuccess) return Ok(response.Value);
        return BadRequest(response.Reasons);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromQuery] ProductDto product, CancellationToken cancellationToken)
    {
        var response = await _productService.CreateProductAsync(product, cancellationToken);
        if (response.IsSuccess) return Created($"api/Products/{product.Id}", product);
        return BadRequest(response.Reasons);
    }
}
