using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.Entities;

public sealed record Product
{
    public long Id { get; init; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; init; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string? Description { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal Price { get; init; }

    public long CategoryId { get; init; }
    public required Category Category { get; init; }
}
