using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public sealed record Category
{
    public long Id { get; init; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; init; }
    public List<Product> Products { get; set; } = new List<Product>();
    public Dictionary<string, string> AdditionalFields { get; init; } = new Dictionary<string, string>();
}
