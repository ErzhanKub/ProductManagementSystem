using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.Contracts.CategoryDto;

public sealed record CategoryPostDto
{

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; set; }
    public Dictionary<string, string>? AdditionalFields { get; init; }
}
