using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Web_Api.Models.Entities;

namespace WebApi.Database;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        var options = new JsonSerializerOptions();

        modelBuilder.Entity<Category>()
            .Property(c => c.AdditionalFields)
            .HasConversion(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, options));

    }
}
