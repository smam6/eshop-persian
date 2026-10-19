using System.Globalization;

namespace eShop.WebAppComponents.Services;

/// <summary>
/// تبدیل و قالب‌بندی تاریخ میلادی به هجری شمسی برای لایهٔ نمایش.
///
/// نکتهٔ مهم: در دیتابیس (PostgreSQL/EF Core) تاریخ‌ها همان
/// <see cref="DateTime"/> میلادی با دقت کامل ذخیره می‌شوند و این کلاس هیچ
/// تغییری در داده‌های ماندگار ایجاد نمی‌کند؛ فقط رشتهٔ نمایشی می‌سازد.
/// </summary>
public static class PersianDateHelper
{
    private static readonly PersianCalendar _persianCalendar = new();

    private static readonly string[] _monthNames =
        ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"];

    private static readonly string[] _dayNames =
        ["یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه"];

    private static readonly TimeZoneInfo? _tehran = ResolveTehranTimeZone();

    /// <summary>تاریخ شمسی، به‌صورت <c>yyyy/MM/dd</c> (یا الگوی خواسته‌شده).</summary>
    public static string ToPersianDateString(this DateTime gregorianDate, string format = "yyyy/MM/dd")
    {
        var date = ToTehran(gregorianDate);
        var year = _persianCalendar.GetYear(date);
        var month = _persianCalendar.GetMonth(date);
        var day = _persianCalendar.GetDayOfMonth(date);

        return format switch
        {
            "yyyy/MM/dd" or "short" => FormattableString.Invariant($"{year:0000}/{month:00}/{day:00}"),
            "yyyy-MM-dd" => FormattableString.Invariant($"{year:0000}-{month:00}-{day:00}"),
            "long" => $"{day} {GetPersianMonthName(month)} {year}",
            "full" => $"{GetPersianDayName(date.DayOfWeek)}، {day} {GetPersianMonthName(month)} {year}",
            _ => FormattableString.Invariant($"{year:0000}/{month:00}/{day:00}")
        };
    }

    /// <summary>تاریخ و ساعت شمسی، مثل <c>۱۴۰۴/۰۷/۲۶ – ۱۸:۴۵</c>.</summary>
    public static string ToPersianDateTimeString(this DateTime gregorianDate)
    {
        var date = ToTehran(gregorianDate);
        var year = _persianCalendar.GetYear(date);
        var month = _persianCalendar.GetMonth(date);
        var day = _persianCalendar.GetDayOfMonth(date);

        return FormattableString.Invariant($"{year:0000}/{month:00}/{day:00} – {date:HH:mm}");
    }

    /// <summary>سال هجری شمسی؛ برای نمایش‌هایی مثل «© ۱۴۰۵».</summary>
    public static int ToPersianYear(this DateTime gregorianDate)
        => _persianCalendar.GetYear(ToTehran(gregorianDate));

    /// <summary>فاصلهٔ زمانی تا امروز به فارسی، برای برچسب «دیروز» و مشابه آن.</summary>
    public static string ToPersianRelativeDays(this DateTime gregorianDate, DateTime? reference = null)
    {
        var today = (reference ?? DateTime.Now).Date;
        var days = (today - gregorianDate.Date).Days;

        return days switch
        {
            <= 0 => "امروز",
            1 => "دیروز",
            < 7 => $"{days} روز پیش",
            < 31 => $"{days / 7} هفته پیش",
            < 365 => $"{days / 30} ماه پیش",
            _ => $"{days / 365} سال پیش"
        };
    }

    /// <summary>نام ماه شمسی (ایندکس ۱ = فروردین).</summary>
    public static string GetPersianMonthName(int month)
        => month is >= 1 and <= 12 ? _monthNames[month - 1] : string.Empty;

    /// <summary>نام روز هفته به فارسی.</summary>
    public static string GetPersianDayName(DayOfWeek dayOfWeek)
        => _dayNames[(int)dayOfWeek];

    /// <summary>
    /// تاریخ‌های UTC/Local به منطقهٔ زمانی تهران برده می‌شوند تا «امروز/دیروز»
    /// و تاریخ سفارش با ساعت کاربر ایرانی هم‌خوان باشد. تاریخ‌های بدون
    /// اطلاعات منطقه (Unspecified) همان‌طور گرفته می‌شوند.
    /// </summary>
    private static DateTime ToTehran(DateTime value) => _tehran switch
    {
        null => value,
        var tz when value.Kind == DateTimeKind.Utc => TimeZoneInfo.ConvertTimeFromUtc(value, tz),
        var tz when value.Kind == DateTimeKind.Local => TimeZoneInfo.ConvertTime(value, tz),
        _ => value
    };

    private static TimeZoneInfo? ResolveTehranTimeZone()
    {
        // لینوکس/کانتینر
        if (TryFind("Asia/Tehran", out var iana))
        {
            return iana;
        }

        // ویندوز
        if (TryFind("Iran Standard Time", out var windows))
        {
            return windows;
        }

        return null;

        static bool TryFind(string id, out TimeZoneInfo? zone)
        {
            try
            {
                zone = TimeZoneInfo.FindSystemTimeZoneById(id);
                return true;
            }
            catch (TimeZoneNotFoundException)
            {
                zone = null;
                return false;
            }
            catch (InvalidTimeZoneException)
            {
                zone = null;
                return false;
            }
        }
    }
}
