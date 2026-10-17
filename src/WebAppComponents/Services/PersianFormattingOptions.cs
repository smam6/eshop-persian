namespace eShop.WebAppComponents.Services;

/// <summary>
/// گزینه‌های قالب‌بندی فارسی (ارقام، واحد پول و رقم اعشار).
/// در WebApp از بخش <c>Persian</c> فایل appsettings خوانده می‌شود و در
/// کلاینت HybridApp مستقیماً در <c>MauiProgram</c> مقداردهی می‌شود.
/// </summary>
public sealed class PersianFormattingOptions
{
    /// <summary>ارقام فارسی (۰ تا ۹) نمایش داده شود؟ با <c>false</c> به لاتین برمی‌گردد.</summary>
    public bool UsePersianDigits { get; set; } = true;

    /// <summary>واحد پول نمایشی؛ «تومان» پیش‌فرض است و «ریال» هم قابل تنظیم است.</summary>
    public string CurrencySymbol { get; set; } = "تومان";

    /// <summary>تعداد رقم اعشار قیمت؛ تومان عملاً واحد خرد رایج ندارد.</summary>
    public int CurrencyDecimals { get; set; }

    /// <summary>واحد پول در نمایش استفاده شود؟ با <c>false</c> فقط عدد چاپ می‌شود.</summary>
    public bool ShowCurrencySymbol { get; set; } = true;
}
