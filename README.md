EShop/
├─ .editorconfig
├─ Directory.Build.props
├─ Directory.Packages.props      ← added today (Step 5)
├─ global.json
├─ EShop.slnx
└─ src/
   ├─ Aspire/
   │  ├─ EShop.AppHost/
   │  └─ EShop.ServiceDefaults/
   └─ Services/
      └─ Catalog/
         └─ Catalog.API/



mkdir EShop && cd EShop
git init
dotnet new gitignore
dotnet new editorconfig
dotnet new globaljson --sdk-version 10.0.100 --roll-forward latestFeature
dotnet new sln -n EShop



[*.cs]
# Namespaces: one line, no extra indentation
csharp_style_namespace_declarations = file_scoped:warning

# Prefer var when the type is obvious
csharp_style_var_when_type_is_apparent = true:suggestion

# Private fields are _camelCase
dotnet_naming_rule.private_fields_underscore.severity = warning
dotnet_naming_rule.private_fields_underscore.symbols = private_fields
dotnet_naming_rule.private_fields_underscore.style = underscore_camel
dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private
dotnet_naming_style.underscore_camel.capitalization = camel_case
dotnet_naming_style.underscore_camel.required_prefix = _



dotnet new aspire-apphost         -n EShop.AppHost         -o src/Aspire/EShop.AppHost
dotnet new aspire-servicedefaults -n EShop.ServiceDefaults -o src/Aspire/EShop.ServiceDefaults
dotnet new web                    -n Catalog.API           -o src/Services/Catalog/Catalog.API


dotnet sln add src/Aspire/EShop.AppHost --solution-folder Aspire
dotnet sln add src/Aspire/EShop.ServiceDefaults --solution-folder Aspire
dotnet sln add src/Services/Catalog/Catalog.API --solution-folder Services/Catalog



dotnet add src/Services/Catalog/Catalog.API reference src/Aspire/EShop.ServiceDefaults
dotnet add src/Aspire/EShop.AppHost reference src/Services/Catalog/Catalog.API


dotnet add src/Aspire/EShop.AppHost package Aspire.Hosting.SqlServer





app.MapGet("/onion", () =>
    {
        Console.WriteLine("    handler");
        return TypedResults.Ok("done");
    })
    .AddEndpointFilter(async (context, next) =>
    {
        Console.WriteLine("A: before");          // on the way in
        var result = await next(context);        // go one layer deeper
        Console.WriteLine("A: after");           // on the way out
        return result;
    })
    .AddEndpointFilter(async (context, next) =>
    {
        Console.WriteLine("  B: before");
        var result = await next(context); // function exe
        Console.WriteLine("  B: after");
        return result;
    });


<!-- A: before
  B: before
    handler
  B: after
A: after -->


double a = 0.1 + 0.2;
Console.WriteLine(a == 0.3); 

# 0.30000000000000004



dotnet add src/Services/Catalog/Catalog.API package Microsoft.AspNetCore.OpenApi
dotnet add src/Services/Catalog/Catalog.API package Scalar.AspNetCore



[FromRoute], [FromQuery], [FromHeader], [FromBody], [FromServices]


dotnet dev-certs https --trust


415