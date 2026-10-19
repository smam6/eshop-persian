using eShop.WebAppComponents.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace eShop.WebApp.Extensions;

/// <summary>
/// پیکربندی محلی‌سازی و جواهره‌سازی (globalization) فارسی برای WebApp.
/// </summary>
public static class LocalizationExtensions
{
    /// <summary>نام بخش پیکربندی مربوط به قالب‌بندی فارسی در appsettings.</summary>
    public const string PersianSectionName = "Persian";

    /// <summary>
    /// ثبت خدمات محلی‌سازی: منابع فارسی + فرهنگ پیش‌فرض fa-IR با تقویم هجری شمسی.
    /// </summary>
    public static IHostApplicationBuilder AddPersianLocalization(this IHostApplicationBuilder builder)
    {
        // یک‌بار برای DI (تا در صورت نیاز با IOptions<> خوانده شود) و یک‌بار
        // به‌صورت مقدارِ Snapshot برای ساختن فرهنگ fa-IR.
        builder.Services.Configure<PersianFormattingOptions>(builder.Configuration.GetSection(PersianSectionName));

        var persianOptions = builder.Configuration
            .GetSection(PersianSectionName)
            .Get<PersianFormattingOptions>() ?? new PersianFormattingOptions();

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        // این پروژه Blazor است و MVC/Razor Pages برای صفحه‌ها ندارد؛ اما
        // AddViewLocalization/AddDataAnnotationsLocalization ثبت می‌شوند تا اگر
        // صفحه‌ای با MVC یا فرمی مبتنی بر DataAnnotations افزوده شد، پیام‌های
        // اعتبارسنجی هم از منابع فارسی خوانده شود.
        builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        builder.Services.AddRazorPages();

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var persianCulture = PersianCultureHelper.GetPersianCulture(persianOptions);
            var englishCulture = PersianCultureHelper.GetEnglishCulture();

            options.DefaultRequestCulture = new RequestCulture(persianCulture);
            options.SupportedCultures = [persianCulture, englishCulture];
            options.SupportedUICultures = [persianCulture, englishCulture];

            // فراهم‌آورنده‌ها: QueryString (?culture=fa-IR) → Cookie → Accept-Language
            options.RequestCultureProviders =
            [
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            ];

            options.FallBackToParentCultures = true;
            options.FallBackToParentUICultures = true;
        });

        return builder;
    }

    /// <summary>
    /// فعال‌سازی <c>UseRequestLocalization</c> و اعمال گزینه‌های فارسی روی
    /// قالب‌سازهای استاتیکی که کامپوننت‌های Razor به‌صورت مستقیم صدا می‌زنند.
    /// </summary>
    public static WebApplication UsePersianLocalization(this WebApplication app)
    {
        var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

        var persianOptions = app.Services.GetService<IOptions<PersianFormattingOptions>>()?.Value;
        if (persianOptions is not null)
        {
            // یک‌بار در شروع به‌کار: همهٔ کامپوننت‌ها همین تنظیمات را به‌کار می‌برند.
            PersianFormat.Configure(persianOptions);
        }

        app.UseRequestLocalization(localizationOptions);
        return app;
    }
}
