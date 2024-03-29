﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Entities;

public sealed record Product
{
    [Key]
    public long Id { get; init; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public required string Name { get; init; }

    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string? Description { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal Price { get; init; }


    public long CategoryId { get; init; }
    public Category Category { get; init; }


    public Dictionary<string, string>? AdditionalFields { get; set; }
}
