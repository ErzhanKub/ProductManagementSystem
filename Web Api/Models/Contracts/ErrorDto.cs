using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.Contracts;

internal sealed record ErrorDto
{
    public int StatusCode { get; set; }

    [Required]
    public string? Message { get; set; }
}