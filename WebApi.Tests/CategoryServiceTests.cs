using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Abstractions.Interfaces;
using WebApi.Models.Contracts.CategoryDto;
using WebApi.Models.Entities;
using WebApi.Services;

namespace WebApi.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoryService _service;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _mapperMock = new Mock<IMapper>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _service = new CategoryService(_repositoryMock.Object, _unitOfWork.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsOkResult_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<Category> { new Category() };
        var categoryDtos = new List<CategoryGetDto> { new CategoryGetDto() { Name = "test" } };
        _repositoryMock.Setup(r => r.GetCategoryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(categories);
        _mapperMock.Setup(m => m.Map<List<CategoryGetDto>>(categories)).Returns(categoryDtos);

        // Act
        var result = await _service.GetCategoriesAsync(CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(categoryDtos, result.Value);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsFailResult_WhenNoCategoriesExist()
    {
        // Arrange
        var categories = new List<Category>();
        _repositoryMock.Setup(r => r.GetCategoryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(categories);

        // Act
        var result = await _service.GetCategoriesAsync(CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("No categories found", result.Reasons.First().Message);
    }

    [Fact]
    public async Task DeleteCategoriesAsync_ReturnsOkResult_WhenCategoriesExist()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category() { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // Act
        var result = await _service.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteCategoriesAsync_ReturnsFailResult_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = 1;
        _repositoryMock.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>())).ReturnsAsync((Category)null!);

        // Act
        var result = await _service.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Category with id {categoryId} does not exist.", result.Reasons.First().Message);
    }

    [Fact]
    public async Task CreateCategoryAsync_ReturnsOkResult_WithCategoryDto()
    {
        // Arreng
        var categoryPostDto = new CategoryPostDto { Name = "test" };
        var category = new Category();
        var categoryGetDto = new CategoryGetDto() { Name = "test" };
        _mapperMock.Setup(m => m.Map<Category>(categoryPostDto)).Returns(category);
        _mapperMock.Setup(m => m.Map<CategoryGetDto>(category)).Returns(categoryGetDto);

        // Act
        var result = await _service.CreateCategoryAsync(categoryPostDto, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(categoryGetDto, result.Value);
    }

    [Fact]
    public async Task CreateCategoryAsync_ThrowsException_WhenAddAsyncFails()
    {
        // Arrange
        var categoryPostDto = new CategoryPostDto() { Name = "test"};
        var category = new Category();
        _mapperMock.Setup(m => m.Map<Category>(categoryPostDto)).Returns(category);
        _repositoryMock.Setup(r => r.AddAsync(category, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateCategoryAsync(categoryPostDto, CancellationToken.None));
    }
}