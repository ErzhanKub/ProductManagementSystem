using AutoMapper;
using FluentResults;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Contracts.ProductDto;
using WebApi.Models.Entities;

namespace WebApi.Services;
/// <summary>
/// Сервис для работы с продуктами.
/// </summary>
public sealed class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// Создать продукт.
    /// </summary>
    /// <param name="productDto">Продукт dto</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<ProductGetDto>> CreateProductAsync(ProductPostDto productDto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(CreateProductAsync)))
        {
            var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId, cancellationToken);

            if (category is null)
            {
                _logger.LogWarning("Attempted to access category: {value}, but it does not exist", productDto.CategoryId);
                return Result.Fail($"Category with id {productDto.CategoryId} does not exist.");
            }
            //ToDo
            if (productDto.AdditionalFields is not null && category.AdditionalFields is not null)
            {
                var invalidFields = productDto.AdditionalFields.Keys.Except(category.AdditionalFields.Keys).ToList();
                if (invalidFields.Any())
                {
                    _logger.LogWarning("Invalid additional fields: {fields}", string.Join(", ", invalidFields));
                }
            }

            var product = _mapper.Map<Product>(productDto);

            _logger.LogInformation("Created product: {id}", product.Id);

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);

            var response = _mapper.Map<ProductGetDto>(product);

            return Result.Ok(response);
        }
    }
    /// <summary>
    /// Получение продукта.
    /// </summary>
    /// <param name="id">Id продукта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<ProductGetDto>> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductByIdAsync)))
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning("Product not found, id: {id}", id);
                return Result.Fail<ProductGetDto>("Product not found");
            }

            var productDto = _mapper.Map<ProductGetDto>(product);

            _logger.LogInformation("Product by ID successfully received: {id}", id);
            return Result.Ok(productDto);
        }
    }
    /// <summary>
    /// Получить продукты по категории.
    /// </summary>
    /// <param name="id">Id категории</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<List<ProductGetDto>>> GetProductsByCategoryAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductsByCategoryAsync)))
        {
            var products = await _productRepository.GetProductsAsync(cancellationToken, id);

            if (!products.Any())
            {
                _logger.LogWarning("No products found");
                return Result.Fail<List<ProductGetDto>>("No products found");
            }

            var productsDto = _mapper.Map<List<ProductGetDto>>(products);

            _logger.LogInformation("Products by category successfully received: {value}", id);
            return Result.Ok(productsDto);
        }
    }
    /// <summary>
    /// Получить продукты по фильтрам.
    /// </summary>
    /// <param name="id">Id категории</param>
    /// <param name="filter">Фильтры поиска <key,value></param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<List<ProductGetDto>>> GetProductsByFilterAsync(long id, Dictionary<string, string> filter, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetProductsByFilterAsync)))
        {
            var products = await _productRepository.GetProductsByFilterAsync(id, filter, cancellationToken);

            if (!products.Any())
            {
                _logger.LogWarning("No products found");
                return Result.Fail<List<ProductGetDto>>("No products found");
            }

            var productsDto = _mapper.Map<List<ProductGetDto>>(products);

            _logger.LogInformation("Filter products successfully received, category id: {id}", id);
            return Result.Ok(productsDto);
        }
    }
}
