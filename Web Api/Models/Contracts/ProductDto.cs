namespace Web_Api.Models.Contracts
{
    public sealed record ProductDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; init; }
        public long CategoryId { get; set; }
        public Dictionary<string, string>? AdditionalFields { get; set; }
    }
}
