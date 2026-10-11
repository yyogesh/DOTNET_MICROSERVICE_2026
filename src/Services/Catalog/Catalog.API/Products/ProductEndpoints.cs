using Catalog.API.Filters;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Products;


public static class ProductEndpoints
{
    private const string GetProductByIdRoute = "GetProductById";

    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .AddEndpointFilter<RequestTimingFilter>();

        group.MapGet("/", GetProducts)
           .WithName("GetProducts")
           .WithSummary("List products")
           .WithDescription("Returns all products, optionally filtered by category and by a search term in the name.");

        group.MapGet("/{id:guid}", GetProductById)
            .WithName(GetProductByIdRoute)
            .WithSummary("Get a product by id")
            .WithDescription("Returns the product with the specified id.");

        group.MapPost("/", CreateProduct)
           .WithName("CreateProduct")
           .WithSummary("Create a product")
           .AddEndpointFilter<PositivePriceFilter>()
           .ProducesValidationProblem();

        
         group.MapPut("/{id:guid}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithSummary("Replace a product")
            .AddEndpointFilter<PositivePriceFilter>()
            .ProducesValidationProblem();

        group.MapDelete("/{id:guid}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithSummary("Delete a product");

        return app;
    }

    private static Ok<IReadOnlyList<Product>> GetProducts(
        [AsParameters] ProductQuery query,
        InMemoryProductStore store) =>
        TypedResults.Ok(store.Find(query.Category, query.Search));

    private static Results<Ok<Product>, NotFound> GetProductById(
        Guid id,
        InMemoryProductStore store) =>
        store.GetById(id) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound();

    // if (p is { Price: > 100, Category: "Shoes" })


    private static CreatedAtRoute<Product> CreateProduct(
       CreateProductRequest request,
       InMemoryProductStore store)
    {
        var product = store.Add(request);
        return TypedResults.CreatedAtRoute(product, GetProductByIdRoute, new { id = product.Id });

        // /api/products/<new id>
    }


      private static Results<NoContent, NotFound> UpdateProduct(
        Guid id,
        UpdateProductRequest request,
        InMemoryProductStore store) =>
        store.Update(id, request)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();

    private static Results<NoContent, NotFound> DeleteProduct(
        Guid id,
        InMemoryProductStore store) =>
        store.Remove(id)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
}