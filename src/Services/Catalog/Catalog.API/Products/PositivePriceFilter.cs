namespace Catalog.API.Products;

// A first, tiny business rule. FluentValidation replaces this on Day 6.
public sealed class PositivePriceFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<IPricedRequest>().FirstOrDefault();

        if (request is { Price: <= 0m })
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["price"] = ["Price must be greater than zero."]
            });
        }

        return await next(context);
    }
}