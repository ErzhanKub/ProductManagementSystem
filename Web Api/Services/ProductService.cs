using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Contracts;
using Web_Api.Models.Entities;

namespace Web_Api.Services;

public sealed class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _productRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<Result<long>> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(CreateProductAsync)))
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId, cancellationToken);

                if (category is null)
                    return Result.Fail("Category not found");

                if (productDto.AdditionalFields != null && category.AdditionalFields != null)
                {
                    foreach (var field in productDto.AdditionalFields.Keys)
                    {
                        if (!category.AdditionalFields.ContainsKey(field))
                        {
                            _logger.LogWarning("Invalid additional field: {field}", field);
                            return Result.Fail($"Invalid additional field: {field}");
                        }
                    }
                }

                var product = new Product
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryId = productDto.CategoryId,
                    AdditionalFields = productDto.AdditionalFields
                };

                _logger.LogInformation("Created product: {id}, date: {time}", productDto.Id, DateTime.Now);

                await _productRepository.AddAsync(product, cancellationToken).ConfigureAwait(false);
                await _unitOfWork.SaveAndCommitAsync(cancellationToken).ConfigureAwait(false);

                return Result.Ok(product.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product id: {id}, date: {time}", productDto.Id, DateTime.Now);
                return Result.Fail<long>("An error occurred while creating the product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product id: {id}, date: {time}", productDto.Id, DateTime.Now);
                return Result.Fail<long>("An error occurred while creating the product");
            }
        }
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductByIdAsync)))
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

                if (product is null)
                {
                    _logger.LogWarning("Product not found, id: {id}, date: {time}", id, DateTime.Now);
                    return Result.Fail<ProductDto>("Product not found");
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    AdditionalFields = product.AdditionalFields,
                };

                _logger.LogInformation("Product by ID successfully received: {id}, date: {time}", id, DateTime.Now);
                return Result.Ok(productDto);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while receiving the product by: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail<ProductDto>("An error occurred while receiving the product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while receiving the product by: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail<ProductDto>("An error occurred while receiving the product");
            }
        }
    }

    public async Task<Result<List<ProductDto>>> GetProductsByCategoryAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductsByCategoryAsync)))
        {
            try
            {
                var products = await _productRepository.GetProductsAsync(cancellationToken, id);

                var productsDto = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    AdditionalFields = p.AdditionalFields,
                }).ToList();

                _logger.LogInformation("Products by category successfully received: {value}", id);
                return Result.Ok(productsDto);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error was received while receiving a product by category id: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail<List<ProductDto>>("An error occurred while receiving the product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error was received while receiving a product by category id: {id}", id);
                return Result.Fail<List<ProductDto>>("An error occurred while receiving the product");
            }
        }
    }

    public async Task<Result<List<ProductDto>>> GetProductsByFilterAsync(long id, Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductsByFilterAsync)))
        {
            try
            {
                var products = await _productRepository.GetProductsByFilterAsync(id, filter, cancellationToken).ConfigureAwait(false);

                var productsDto = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    AdditionalFields = p.AdditionalFields,
                }).ToList();

                _logger.LogInformation("Filter products successfully received, category id: {id}, date: {time}", id, DateTime.Now);
                return Result.Ok(productsDto);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error was received while receiving goods by filter id: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail<List<ProductDto>>("An error was received while receiving goods by filter");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error was received while receiving goods by filter id: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail<List<ProductDto>>("An error was received while receiving goods by filter");
            }
        }
    }
}
