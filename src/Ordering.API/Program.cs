using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

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

var orders = app.NewVersionedApi("Orders");

orders.MapOrdersApiV1()
      .RequireAuthorization();

app.UseDefaultOpenApi();
app.Run();
