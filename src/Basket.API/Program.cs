using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

app.MapDefaultEndpoints();

var supportedCultures = new[] { new CultureInfo("fa-IR"), new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fa-IR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapGrpcService<BasketService>();

app.Run();
