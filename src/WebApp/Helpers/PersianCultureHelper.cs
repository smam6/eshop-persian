using System.Globalization;

namespace eShop.WebApp.Helpers;

/// <summary>
/// کلاس کمکی برای ایجاد فرهنگ سفارشی فارسی با تقویم هجری شمسی.
/// این کلاس نمایش تاریخ/عدد/قیمت را در سراسر برنامه یکسان می‌کند
/// در حالی که ذخیرهٔ داده در دیتابیس همچنان بر مبنای DateTime میلادی (UTC) باقی می‌ماند.
/// </summary>
public static class PersianCultureHelper
{
    /// <summary>
    /// یک نمونهٔ CultureInfo برای fa-IR با PersianCalendar و واحد پول تومان برمی‌گرداند.
    /// </summary>
    public static CultureInfo GetPersianCulture()
    {
        // کپی قابل‌نوشتن از فرهنگ فارسی – تغییرات روی نمونهٔ اصلی اثر نمی‌گذارد.
        var culture = new CultureInfo("fa-IR");

        // تلاش برای اعمال تقویم هجری شمسی؛ در صورت عدم پشتیبانی، به فرهنگ پیش‌فرض بازمی‌گردیم.
        try
        {
            culture.DateTimeFormat.Calendar = new PersianCalendar();

            // ترتیب تاریخ شمسی: yyyy/MM/dd – سازگار با الگوهای رایج در ایران
            culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            culture.DateTimeFormat.LongDatePattern = "dddd، d MMMM yyyy";
            culture.DateTimeFormat.DateSeparator = "/";

            // نام ماه‌های شمسی به فارسی
            culture.DateTimeFormat.MonthNames = [
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
            ];
            culture.DateTimeFormat.AbbreviatedMonthNames = [
                "فرو", "ارد", "خرد", "تیر", "مرد", "شهر", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", ""
            ];
            culture.DateTimeFormat.MonthGenitiveNames = culture.DateTimeFormat.MonthNames;
            culture.DateTimeFormat.AbbreviatedMonthGenitiveNames = culture.DateTimeFormat.AbbreviatedMonthNames;

            culture.DateTimeFormat.DayNames = ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"];
            culture.DateTimeFormat.AbbreviatedDayNames = ["ی", "د", "س", "چ", "پ", "ج", "ش"];
            culture.DateTimeFormat.ShortestDayNames = culture.DateTimeFormat.AbbreviatedDayNames;

            culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Saturday;
        }
        catch (ArgumentOutOfRangeException)
        {
            // برخی محیط‌ها PersianCalendar را برای fa-IR نمی‌پذیرند – بدون خطا ادامه می‌دهیم.
        }

        // قالب اعداد فارسی – جداکنندهٔ هزارگان و اعشار بومی
        culture.NumberFormat.NumberDecimalSeparator = "٫";  // ممیز فارسی U+066B
        culture.NumberFormat.NumberGroupSeparator = "٬";     // جداکنندهٔ هزارگان فارسی U+066C
        culture.NumberFormat.NumberNegativePattern = 1;       // -n

        // نمایش ارقام: برای خوانایی در وب، ارقام لاتین نگه داشته می‌شود؛
        // در صورت نیاز به ارقام فارسی، DigitSubstitution را به Native تغییر دهید
        // و از فونت Vazirmatn که ارقام فارسی دارد استفاده کنید.

        // واحد پول: تومان (به‌جای ریال پیش‌فرض)
        culture.NumberFormat.CurrencySymbol = "تومان";
        culture.NumberFormat.CurrencyDecimalSeparator = "٫";
        culture.NumberFormat.CurrencyGroupSeparator = "٬";
        culture.NumberFormat.CurrencyPositivePattern = 3; // n تومان
        culture.NumberFormat.CurrencyNegativePattern = 8; // -n تومان

        return culture;
    }

    /// <summary>
    /// فهرست فرهنگ‌های پشتیبانی‌شده: فارسی (پیش‌فرض) و انگلیسی (Fallback)
    /// </summary>
    public static CultureInfo[] GetSupportedCultures() =>
    [
        GetPersianCulture(),
        new CultureInfo("en-US")
    ];
}
