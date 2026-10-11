namespace Catalog.API.Products;

public interface IPricedRequest
{
    decimal Price { get; }
}

public sealed record CreateProductRequest(
    string Name,
    string Description,
    string Category,
    decimal Price,
    string? ImageUrl) : IPricedRequest;


public sealed record UpdateProductRequest(
    string Name,
    string Description,
    string Category,
    decimal Price,
    string? ImageUrl) : IPricedRequest;


// Query-string parameters for GET /api/products, bound with [AsParameters]
public sealed record ProductQuery(string? Category, string? Search);