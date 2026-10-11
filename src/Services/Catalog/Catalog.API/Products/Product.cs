namespace Catalog.API.Products;

public sealed record Product(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    string? ImageUrl);