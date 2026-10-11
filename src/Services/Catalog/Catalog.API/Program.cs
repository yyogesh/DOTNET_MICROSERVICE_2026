using Catalog.API.Products;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options =>
{
     options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new()
        {
            Title = "EShop Catalog API",
            Version = "v1",
            Description = "Products for the EShop course"
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddSingleton<InMemoryProductStore>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenApi("/openapi/{documentName}.yaml");
    app.MapScalarApiReference(options => options.WithTitle("EShop Catalog API"));
}

app.MapDefaultEndpoints();

// app.MapGet("/", () => "Hello World!");

app.MapGet("/", (IConfiguration config) => TypedResults.Ok(new
{
    Service = "Catalog.API",
    Day = 1,
    DatabaseConfigured = !string.IsNullOrEmpty(config.GetConnectionString("catalogdb"))
}))
.ExcludeFromDescription();

app.MapProductEndpoints();

app.Run();
