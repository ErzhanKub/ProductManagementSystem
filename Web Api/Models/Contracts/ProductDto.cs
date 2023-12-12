namespace Web_Api.Models.Contracts
{
    internal sealed record ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
