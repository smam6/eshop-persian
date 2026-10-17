using System.Globalization;

namespace eShop.ClientApp;

/// <summary>
/// تنظیم فرهنگ فارسی برای کلاینت موبایل (MAUI).
///
/// این کلاس عمداً مستقل از WebAppComponents است؛ کلاینت موبایل به آن پروژه
/// ارجاع نمی‌دهد. کارکرد:
///  • تقویم هجری شمسی برای همهٔ <c>ToString()</c>های تاریخ (از جمله
///    <c>StringFormat='{0:MMMM dd yyyy}'</c> در XAML).
///  • واحد پول تومان با الگوی «عدد سپس واحد» تا <c>{0:C}</c> در XAML بدون
///    پیشوند دلاری رندر شود.
///  • جداکننده‌های فارسی هزارگان و ممیز.
/// داده‌ها در سرویس‌ها/دیتابیس همچنان میلادی و عددی ذخیره می‌شوند.
/// </summary>
public static class PersianGlobalization
{
    public const string CurrencySymbol = "تومان";

    /// <summary>
    /// فرهنگ پیش‌فرض رشته‌ها را روی fa-IR با تقویم شمسی تنظیم می‌کند.
    /// باید پیش از ساخت برنامه (MauiApp.CreateBuilder) صدا زده شود.
    /// </summary>
    public static void Apply()
    {
        var culture = TryCreateCulture();
        if (culture is null)
        {
            // محیطی که اصلاً دادهٔ فرهنگ ندارد (Globalization-Invariant)؛
            // برنامه با فرهنگ پیش‌فرض بالا می‌ماند و فارسی‌ها از XAML می‌آیند.
            return;
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    private static CultureInfo? TryCreateCulture()
    {
        try
        {
            var culture = new CultureInfo("fa-IR");

            culture.DateTimeFormat.Calendar = new PersianCalendar();
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            culture.DateTimeFormat.LongDatePattern = "dddd، d MMMM yyyy";
            culture.DateTimeFormat.DateSeparator = "/";
            culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;
            culture.DateTimeFormat.DayNames =
                ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"];
            culture.DateTimeFormat.AbbreviatedDayNames = ["ی", "د", "س", "چ", "پ", "ج", "ش"];
            culture.DateTimeFormat.ShortestDayNames = culture.DateTimeFormat.AbbreviatedDayNames;
            culture.DateTimeFormat.MonthNames =
            [
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty
            ];
            culture.DateTimeFormat.MonthGenitiveNames = culture.DateTimeFormat.MonthNames;

            culture.NumberFormat.NumberDecimalSeparator = "٫";
            culture.NumberFormat.NumberGroupSeparator = "٬";
            culture.NumberFormat.CurrencySymbol = CurrencySymbol;
            culture.NumberFormat.CurrencyDecimalSeparator = "٫";
            culture.NumberFormat.CurrencyGroupSeparator = "٬";
            culture.NumberFormat.CurrencyDecimalDigits = 0;
            culture.NumberFormat.CurrencyPositivePattern = 3; // n L
            culture.NumberFormat.CurrencyNegativePattern = 1; // -n L

            return culture;
        }
        catch (Exception exception) when (
            exception is CultureNotFoundException
                or ArgumentException
                or NotSupportedException
                or InvalidOperationException)
        {
            return null;
        }
    }
}
