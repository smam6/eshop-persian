using System.Globalization;

namespace eShop.WebAppComponents.Services;

/// <summary>
/// سازندهٔ فرهنگ سفارشی <c>fa-IR</c> با تقویم هجری شمسی و واحد پول تومان.
///
/// همین فرهنگ در <c>RequestLocalizationOptions.SupportedCultures</c> ثبت می‌شود تا
/// <c>DateTime.ToString()</c> و <c>decimal.ToString()</c> در سراسر رابط کاربری خودکار
/// شمسی/فارسی شوند؛ داده‌ها در لایهٔ ماندگاری همان <see cref="DateTime"/> میلادی (UTC)
/// و <see cref="decimal"/> می‌مانند و فقط لایهٔ نمایش تغییر می‌کند.
///
/// ارقام عمداً لاتین نگه داشته می‌شوند تا ورودی‌های عددی و تبدیل رشته→عدد در فرم‌ها
/// بی‌خطر بماند؛ تبدیل به رقم فارسی در لایهٔ نمایش و با
/// <see cref="PersianFormat.ToPersianDigits(string, bool?)"/> انجام می‌شود.
/// </summary>
public static class PersianCultureHelper
{
    /// <summary>ممیز فارسی (U+066B).</summary>
    public const string PersianDecimalSeparator = "٫";

    /// <summary>جداکنندهٔ هزارگان فارسی (U+066C).</summary>
    public const string PersianGroupSeparator = "٬";

    /// <summary>الگوی تاریخ کوتاه شمسی، سازگار با عرف رایج در ایران.</summary>
    public const string ShortDatePattern = "yyyy/MM/dd";

    private static readonly CultureInfo _englishCulture = TryCreateCulture("en-US") ?? CultureInfo.InvariantCulture;
    private static CultureInfo? _persianCulture;
    private static readonly object _sync = new();

    /// <summary>
    /// فرهنگ فارسی. اگر <paramref name="options"/> داده نشود، نسخهٔ کش‌شدهٔ پیش‌فرض
    /// برگردانده می‌شود (برای پیکربندی RequestLocalization همین کافی است).
    /// </summary>
    public static CultureInfo GetPersianCulture(PersianFormattingOptions? options = null)
    {
        if (options is null)
        {
            lock (_sync)
            {
                return _persianCulture ??= CreatePersianCulture(new PersianFormattingOptions());
            }
        }

        return CreatePersianCulture(options);
    }

    /// <summary>فرهنگ انگلیسی (fallback) تا با تغییر زبان، قالب تاریخ/عدد هم عوض شود.</summary>
    public static CultureInfo GetEnglishCulture() => _englishCulture;

    /// <summary>فهرست فرهنگ‌های پشتیبانی‌شده: فارسی (پیش‌فرض) و انگلیسی (fallback).</summary>
    public static CultureInfo[] GetSupportedCultures(PersianFormattingOptions? options = null) =>
    [
        GetPersianCulture(options),
        GetEnglishCulture()
    ];

    private static CultureInfo CreatePersianCulture(PersianFormattingOptions options)
    {
        // در حالت Globalization-Invariant (برخی ایمیج‌های کوچک‌شده) فرهنگ واقعی
        // ساخته نمی‌شود؛ در آن حالت از اینواریانتِ قابل‌ویرایش شروع می‌کنیم تا
        // باز هم جداکننده‌ها و تقویم شمسی اعمال شوند و برنامه بالا بیاید.
        var culture = TryCreateCulture("fa-IR") ?? (CultureInfo)CultureInfo.InvariantCulture.Clone();

        if (culture.DateTimeFormat.IsReadOnly || culture.NumberFormat.IsReadOnly)
        {
            culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        }

        ApplyDateTimeFormat(culture);
        ApplyNumberFormat(culture, options);

        return culture;
    }

    private static CultureInfo? TryCreateCulture(string name)
    {
        try
        {
            return new CultureInfo(name);
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    /// تقویم شمسی، نام ماه‌ها/روزها و الگوهای تاریخ. اگر محیط تقویم شمسی را
    /// نپذیرد، بی‌خطا رد می‌شویم؛ تبدیل تاریخ در <see cref="PersianDateHelper"/>
    /// مستقل از این موضوع کار می‌کند.
    /// </summary>
    private static void ApplyDateTimeFormat(CultureInfo culture)
    {
        var format = culture.DateTimeFormat;

        try
        {
            format.Calendar = new PersianCalendar();
        }
        catch (Exception ex) when (ex is ArgumentOutOfRangeException or ArgumentException or NotSupportedException)
        {
            return;
        }

        format.ShortDatePattern = ShortDatePattern;
        format.LongDatePattern = "dddd، d MMMM yyyy";
        format.FullDateTimePattern = "yyyy/MM/dd HH:mm:ss";
        format.DateSeparator = "/";
        format.FirstDayOfWeek = DayOfWeek.Saturday; // هفته در ایران از شنبه شروع می‌شود

        // آرایه‌ها باید از یکشنبه (DayOfWeek = 0) شروع شوند.
        format.DayNames = ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"];
        format.AbbreviatedDayNames = ["ی", "د", "س", "چ", "پ", "ج", "ش"];
        format.ShortestDayNames = format.AbbreviatedDayNames;

        format.MonthNames =
        [
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty
        ];
        format.MonthGenitiveNames = format.MonthNames;

        format.AbbreviatedMonthNames =
        [
            "فرو", "ارد", "خرد", "تیر", "مرد", "شهر", "مهر", "آبا", "آذر", "دی", "بهم", "اسف", string.Empty
        ];
        format.AbbreviatedMonthGenitiveNames = format.AbbreviatedMonthNames;

        format.AMDesignator = "ق.ظ";
        format.PMDesignator = "ب.ظ";
    }

    private static void ApplyNumberFormat(CultureInfo culture, PersianFormattingOptions options)
    {
        var format = culture.NumberFormat;

        format.NumberDecimalSeparator = PersianDecimalSeparator;
        format.NumberGroupSeparator = PersianGroupSeparator;
        format.NumberNegativePattern = 1; // -n

        var symbol = string.IsNullOrWhiteSpace(options.CurrencySymbol) ? "تومان" : options.CurrencySymbol.Trim();
        var decimals = Math.Max(0, options.CurrencyDecimals);

        format.CurrencySymbol = symbol;
        format.CurrencyDecimalSeparator = PersianDecimalSeparator;
        format.CurrencyGroupSeparator = PersianGroupSeparator;
        format.CurrencyDecimalDigits = decimals;
        format.CurrencyPositivePattern = 3; // n L  →  «۱٬۲۳۰ تومان»
        format.CurrencyNegativePattern = 1; // -n L
    }
}
