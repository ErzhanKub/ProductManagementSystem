using Microsoft.AspNetCore.Mvc;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Entities;

namespace Web_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetAsync(cancellationToken);
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
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
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
            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);
            _logger.LogInformation("product created: {value}", product.Id);
            return Created($"api/Products/{product.Id}", product);
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

            _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);
            _logger.LogInformation("Product data updated successfully: {value}", product.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while updating data", ex);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _productRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);
            _logger.LogInformation("deleted product: {value}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the product");
            throw;
        }
    }
}
