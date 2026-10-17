using System.Globalization;
using eShop.WebApp.Helpers;
using Microsoft.AspNetCore.Localization;

namespace eShop.WebApp.Extensions;

public static class LocalizationExtensions
{
    /// <summary>
    /// پیکربندی محلی‌سازی فارسی برای WebApp
    /// - منابع در پوشهٔ Resources
    /// - فرهنگ پیش‌فرض fa-IR با تقویم شمسی
    /// - پشتیبانی از en-US به‌عنوان Fallback
    /// </summary>
    public static IHostApplicationBuilder AddPersianLocalization(this IHostApplicationBuilder builder)
    {
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        builder.Services.AddRazorPages();

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var persianCulture = PersianCultureHelper.GetPersianCulture();
            var englishCulture = new CultureInfo("en-US");

            options.DefaultRequestCulture = new RequestCulture(persianCulture);
            options.SupportedCultures = [persianCulture, englishCulture];
            options.SupportedUICultures = [persianCulture, englishCulture];

            // فراهم‌آورنده‌ها: QueryString (?culture=fa-IR) → Cookie → Accept-Language
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };

            options.FallBackToParentCultures = true;
            options.FallBackToParentUICultures = true;
        });

        return builder;
    }

    public static WebApplication UsePersianLocalization(this WebApplication app)
    {
        var locOptions = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(locOptions);
        return app;
    }
}
