namespace Web_Api.Models.Contracts;

public sealed record CategoryDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public Dictionary<string, string>? AdditionalFields { get; init; }
}
