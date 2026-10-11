using System.Collections.Concurrent;

namespace Catalog.API.Products;

public sealed class InMemoryProductStore
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public InMemoryProductStore()
    {
        Seed(new Product(Guid.Parse("7a1c3e5f-2b4d-4f6a-8c9e-0a1b2c3d4e01"),
            "Trail Runner 2", "Lightweight trail running shoe with a grippy outsole.",
            "Shoes", 129.99m, null));
        Seed(new Product(Guid.Parse("7a1c3e5f-2b4d-4f6a-8c9e-0a1b2c3d4e02"),
            "City Sneaker", "Everyday leather sneaker.",
            "Shoes", 89.50m, null));
        Seed(new Product(Guid.Parse("7a1c3e5f-2b4d-4f6a-8c9e-0a1b2c3d4e03"),
            "Merino Crew Socks (3 pack)", "Breathable socks for running and hiking.",
            "Apparel", 24.00m, null));
        Seed(new Product(Guid.Parse("7a1c3e5f-2b4d-4f6a-8c9e-0a1b2c3d4e04"),
            "Waterproof Shell Jacket", "Packable rain jacket with taped seams.",
            "Apparel", 199.00m, null));
    }

    public IReadOnlyList<Product> Find(string? category, string? search)
    {
        IEnumerable<Product> query = _products.Values;

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return [.. query.OrderBy(p => p.Name)];
    }

    public Product? GetById(Guid id) =>
        _products.TryGetValue(id, out var product) ? product : null;

    public Product Add(CreateProductRequest request)
    {
        var product = new Product(Guid.NewGuid(), request.Name, request.Description,
            request.Category, request.Price, request.ImageUrl);
        _products[product.Id] = product;
        return product;
    }


    public bool Update(Guid id, UpdateProductRequest request)
    {
        if (!_products.ContainsKey(id))
        {
            return false;
        }

        _products[id] = new Product(id, request.Name, request.Description,
            request.Category, request.Price, request.ImageUrl);
        return true;
    }

    public bool Remove(Guid id) => _products.TryRemove(id, out _);

    private void Seed(Product product) => _products[product.Id] = product;

}