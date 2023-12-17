using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Web_Api.Abstractions.Interfaces;
using Web_Api.Models.Contracts;
using Web_Api.Models.Entities;

namespace Web_Api.Services;
/// <summary>
/// Сервис для рабоыт с категориями.
/// </summary>
public sealed class CategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    /// <summary>
    /// Асинхронный метод для получние всех категории.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<List<CategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(GetCategoriesAsync)))
        {
            try
            {
                var categories = await _repository.GetCategoryAsync(cancellationToken);
                var categoriesDto = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    AdditionalFields = c.AdditionalFields,
                }).ToList();

                _logger.LogInformation("Categories successfully received, date: {time}", DateTime.Now);

                return Result.Ok(categoriesDto);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the category, date: {time}", DateTime.Now);
                return Result.Fail<List<CategoryDto>>("An error occurred while retrieving the category");
            }
        }
    }
    /// <summary>
    /// Асинхронный метод для удаления категории по его id.
    /// </summary>
    /// <param name="id">Id категории</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result> DeleteCategoryAsync(long id, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(DeleteCategoryAsync)))
        {
            try
            {
                await _repository.DeleteAsync(id, cancellationToken);
                await _unitOfWork.SaveAndCommitAsync(cancellationToken);

                _logger.LogInformation("Deleted category: {value}, date: {time}", id, DateTime.Now);

                return Result.Ok();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category, id: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail("An error occurred during deletion");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category, id: {id}, date: {time}", id, DateTime.Now);
                return Result.Fail("An error occurred during deletion");
            }
        }
    }
    /// <summary>
    /// Асинхронный метод для создание категории.
    /// </summary>
    /// <param name="categoryDto">Обьект категории dto</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    public async Task<Result<long>> CreateCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(CreateCategoryAsync)))
        {
            try
            {
                var category = new Category
                {
                    Id = categoryDto.Id,
                    Name = categoryDto.Name,
                    AdditionalFields = categoryDto.AdditionalFields
                };
                await _repository.AddAsync(category, cancellationToken);
                await _unitOfWork.SaveAndCommitAsync(cancellationToken);

                _logger.LogInformation("Created category: {id}, date: {time}", category.Id, DateTime.Now);
                return Result.Ok(category.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating the category id: {id}, date: {time}", categoryDto.Id, DateTime.Now);
                return Result.Fail<long>("An error occurred while creating the category");
            }
        }
    }
}
