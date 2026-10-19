using Microsoft.Extensions.FileProviders;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.AddApplicationServices();
builder.Services.AddKeyedSingleton<IFileProvider>("pics", (services, _) =>
    new PhysicalFileProvider(Path.Combine(
        services.GetRequiredService<IWebHostEnvironment>().ContentRootPath, "Pics")));
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning(options =>
{
    // Include "api-supported-versions" and "api-deprecated-versions" headers in all responses
    options.ReportApiVersions = true;
});

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

var supportedCultures = new[] { new CultureInfo("fa-IR"), new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fa-IR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStatusCodePages();

app.MapCatalogApi();

app.UseDefaultOpenApi();
app.Run();
