var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

// app.MapGet("/", () => "Hello World!");

app.MapGet("/", (IConfiguration config) => TypedResults.Ok(new
{
    Service = "Catalog.API",
    Day = 1,
    DatabaseConfigured = !string.IsNullOrEmpty(config.GetConnectionString("catalogdb"))
}));

app.Run();
