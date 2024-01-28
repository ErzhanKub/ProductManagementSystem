using AutoMapper;
using FluentResults;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Contracts.CategoryDto;
using WebApi.Models.Entities;

namespace WebApi.Services;
public sealed class CategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryGetDto>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetCategoriesAsync)))
        {
            var categories = await _repository.GetCategoryAsync(cancellationToken);

            if (!categories.Any())
            {
                _logger.LogWarning("No categories found");
                return Result.Fail<List<CategoryGetDto>>("No categories found");
            }

            var categoriesDto = _mapper.Map<List<CategoryGetDto>>(categories);

            _logger.LogInformation("Categories successfully received, count: {count}", categoriesDto.Count);

            return Result.Ok(categoriesDto);
        }
    }

    public async Task<Result> DeleteCategoryAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(DeleteCategoryAsync)))
        {
            var category = await _repository.GetByIdAsync(id, cancellationToken);

            if (category is null)
            {
                _logger.LogWarning("Attempted to delete category: {value}, but it does not exist", id);
                return Result.Fail($"Category with id {id} does not exist.");
            }

            await _repository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);

            _logger.LogInformation("Deleted category: {value}", id);

            return Result.Ok();
        }
    }

    public async Task<Result<CategoryGetDto>> CreateCategoryAsync(CategoryPostDto categoryDto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(CreateCategoryAsync)))
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _repository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);

            _logger.LogInformation("Created category: {id}, name: {name}", category.Id, category.Name);

            var response = _mapper.Map<CategoryGetDto>(category);
            return Result.Ok(response);
        }
    }
}
