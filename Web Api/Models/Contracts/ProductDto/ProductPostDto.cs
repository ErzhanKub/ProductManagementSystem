using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.Contracts.ProductDto;

public sealed record ProductPostDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal Price { get; init; }
    public long CategoryId { get; set; }
    public Dictionary<string, string>? AdditionalFields { get; set; }
}
