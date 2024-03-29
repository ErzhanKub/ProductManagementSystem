﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Contracts.CategoryDto;

public sealed record CategoryGetDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; set; }
    public Dictionary<string, string>? AdditionalFields { get; init; }
}
