using System.Globalization;

namespace eShop.WebAppComponents.Services;

/// <summary>
/// نقطهٔ واحد قالب‌بندی «نمایشی» فارسی: رقم‌ها، عدد، قیمت و تاریخ.
///
/// چرا این کلاس لازم است؟
///  • فرهنگ fa-IR در دات‌نت بستهٔ به ICU، ارقام را گاه لاتین و گاه فارسی می‌دهد؛
///    برای یک رابط کاربری پایدار، رقم‌ها را صریحاً خودمان تبدیل می‌کنیم و به
///    <c>DigitSubstitution</c> وابسته نمی‌شویم.
///  • تبدیل تاریخ با <see cref="PersianCalendar"/> انجام می‌شود (نه با
///    <c>DateTimeFormat.Calendar</c>) تا خروجی در همهٔ سکو‌ها (از جمله
///    AOT/InvariantGlobalization) یکسان بماند.
///  • داده‌ها در لایهٔ ماندگاری همان <see cref="DateTime"/> میلادی (UTC) و
///    <see cref="decimal"/> می‌مانند؛ این کلاس فقط رشتهٔ نمایشی می‌سازد.
/// </summary>
public static class PersianFormat
{
    /// <summary>جداکنندهٔ هزارگان فارسی (U+066C).</summary>
    public const char GroupSeparator = '٬';

    /// <summary>ممیز فارسی (U+066B).</summary>
    public const char DecimalSeparator = '٫';

    /// <summary>
    /// واحد پول نمایشی. در <c>Program.cs</c> از بخش پیکربندی
    /// <c>Persian:CurrencySymbol</c> قابل تغییر است (مثلاً «ریال»).
    /// </summary>
    public static string CurrencySymbol { get; set; } = "تومان";

    /// <summary>
    /// کلید سراسری نمایش ارقام فارسی. با <c>Persian:UsePersianDigits=false</c>
    /// می‌توان به ارقام لاتین برگشت (بدون تغییر در کد رابط کاربری).
    /// </summary>
    public static bool UsePersianDigits { get; set; } = true;

    /// <summary>فاصلهٔ معمولی میان عدد و واحد پول («۱٬۲۳۰ تومان»).</summary>
    private const string CurrencySeparator = " ";

    /// <summary>
    /// رقم اعشار قیمت. برای تومان صفر است (واحد خرد رایج نداریم) و با
    /// <c>Persian:CurrencyDecimals</c> قابل تغییر است.
    /// </summary>
    public static int CurrencyDecimals { get; set; }

    /// <summary>واحد پول در انتها نمایش داده شود؟</summary>
    public static bool ShowCurrencySymbol { get; set; } = true;

    /// <summary>
    /// گزینه‌های قالب‌بندی را از پیکربندی به این قالب‌ساز استاتیک منتقل می‌کند.
    /// در WebApp هنگام راه‌اندازی (UsePersianLocalization) و در HybridApp در
    /// MauiProgram صدا زده می‌شود.
    /// </summary>
    public static void Configure(PersianFormattingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        UsePersianDigits = options.UsePersianDigits;
        CurrencySymbol = string.IsNullOrWhiteSpace(options.CurrencySymbol) ? "تومان" : options.CurrencySymbol.Trim();
        CurrencyDecimals = Math.Max(0, options.CurrencyDecimals);
        ShowCurrencySymbol = options.ShowCurrencySymbol;
    }

    /// <summary>آیا فرهنگ داده‌شده (پیش‌فرض: فرهنگ جاری) فارسی است؟</summary>
    public static bool IsPersian(CultureInfo? culture = null)
        => (culture ?? CultureInfo.CurrentCulture).Name.StartsWith("fa", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// رقم‌های لاتین یک رشته را به رقم‌های فارسی تبدیل می‌کند و جداکننده‌های
    /// انگلیسی را با معادل فارسی جایگزین می‌کند. بقیهٔ کاراکترها دست‌نخورده می‌مانند.
    /// </summary>
    public static string ToPersianDigits(string? value, bool? usePersianDigits = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }

        if (!(usePersianDigits ?? UsePersianDigits))
        {
            return value;
        }

        // رشته‌های نمایشی کوتاه‌اند؛ یک آرایهٔ محلی ساده‌ترین و بی‌خطرترین راه است.
        var buffer = new char[value.Length];
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            buffer[i] = c switch
            {
                >= '0' and <= '9' => (char)('۰' + (c - '0')),
                ',' => GroupSeparator,
                '.' => DecimalSeparator,
                _ => c
            };
        }

        return new string(buffer);
    }

    /// <summary>
    /// رقم‌های فارسی/عربی یک رشتهٔ ورودی را به لاتین تبدیل می‌کند تا بتوان
    /// ورودی کاربر را (مثل تاریخ انقضای کارت یا کد پستی) با فرهنگ اینواریانت
    /// تجزیه کرد. جداکننده‌های فارسی هم به معادل ASCII خود برگردانده می‌شوند.
    /// </summary>
    public static string ToLatinDigits(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }

        var buffer = new char[value.Length];
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            buffer[i] = c switch
            {
                >= '\u06F0' and <= '\u06F9' => (char)('0' + (c - '\u06F0')), // رقم فارسی ۰-۹
                >= '\u0660' and <= '\u0669' => (char)('0' + (c - '\u0660')), // رقم عربی ٠-٩
                '\u066B' => '.',   // ممیز فارسی
                '\u066C' => ',',   // جداکنندهٔ هزارگان فارسی
                _ => c
            };
        }

        return new string(buffer);
    }

    /// <summary>
    /// عدد را با جداکنندهٔ هزارگان و تعداد رقم اعشار خواسته‌شده قالب‌بندی می‌کند.
    /// </summary>
    public static string FormatNumber(
        decimal value,
        int decimals = 0,
        CultureInfo? culture = null,
        bool? usePersianDigits = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        var isPersian = IsPersian(culture);
        var persianDigits = usePersianDigits ?? (isPersian && UsePersianDigits);

        // الگوی N{decimals} با فرهنگ جاری: جداکنندهٔ هزارگان و ممیز درست انتخاب می‌شود.
        var pattern = decimals > 0 ? $"N{decimals}" : "N0";
        var text = value.ToString(pattern, culture);

        // در حالت انگلیسی، جداکننده‌ها باید همسان رفتار قبلی (N2 اینواریانت) بمانند.
        return isPersian ? ToPersianDigits(text, persianDigits) : text;
    }

    /// <summary>
    /// قیمت را برای نمایش در رابط کاربری می‌سازد:
    ///  • فارسی: <c>۱٬۲۳۰ تومان</c>
    ///  • غیر فارسی: <c>$1,230.00</c> (همان رفتار پیشین eShop)
    /// </summary>
    public static string FormatCurrency(decimal value, CultureInfo? culture = null, bool? usePersianDigits = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (!IsPersian(culture))
        {
            return "$" + value.ToString("N2", CultureInfo.InvariantCulture);
        }

        var number = FormatNumber(Math.Round(value, CurrencyDecimals, MidpointRounding.AwayFromZero),
            CurrencyDecimals, culture, usePersianDigits);

        return ShowCurrencySymbol && CurrencySymbol.Length > 0
            ? string.Concat(number, CurrencySeparator, CurrencySymbol)
            : number;
    }

    /// <summary>
    /// تاریخ را در تقویم هجری شمسی نمایش می‌دهد (فارسی) یا تقویم پیش‌فرض فرهنگ (سایر زبان‌ها).
    /// </summary>
    public static string FormatDate(DateTime date, CultureInfo? culture = null, bool withTime = false, bool? usePersianDigits = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (!IsPersian(culture))
        {
            return withTime ? date.ToString("g", culture) : date.ToString("d", culture);
        }

        var persianDigits = usePersianDigits ?? UsePersianDigits;
        var text = withTime ? date.ToPersianDateTimeString() : date.ToPersianDateString();

        return ToPersianDigits(text, persianDigits);
    }
}
