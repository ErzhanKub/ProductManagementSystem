using Microsoft.AspNetCore.Mvc;
using Web_Api.Interfaces;
using WebApi.Models;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetProductsAsync(cancellationToken);
            _logger.LogInformation("All products received successfully");
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while receiving all products");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(long id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetProductByIdAsync(id, cancellationToken);
            _logger.LogInformation("Product by ID successfully received: {value}", id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while receiving the product by id");
            throw;
        }
    }

    [HttpGet("bycategory/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(long categoryId, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId, cancellationToken);
            _logger.LogInformation("Products by category successfully received: {value}", categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error was received while receiving a product by category");
            throw;
        }
    }

    [HttpGet("byfilter")]
    public async Task<IActionResult> GetProductsByFilter([FromQuery] long categoryId, [FromQuery] Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetProductsByFilterAsync(categoryId, filter, cancellationToken);
            _logger.LogInformation("Filter products successfully received");
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error was received while receiving goods by filter");
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromQuery] Product product, CancellationToken cancellationToken)
    {
        try
        {
            await _productRepository.AddProductAsync(product, cancellationToken);
            _logger.LogInformation("product created: {value}", product.Id);
            return Created($"api/Products/{product.Id}",product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during product creation");
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(long id, [FromQuery] Product product, CancellationToken cancellationToken)
    {
        try
        {
            if (id != product.Id)
                return BadRequest();

            _productRepository.UpdateProductAsync(product, cancellationToken);
            return NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}
