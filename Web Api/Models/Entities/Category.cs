using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.Entities;

public sealed record Category
{
    [Key]
    public long Id { get; init; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; init; }

    public List<Product> Products { get; init; } = new();

    public Dictionary<string, string> AdditionalFields { get; init; } = new();
}
