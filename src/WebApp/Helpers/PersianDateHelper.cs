using System.Globalization;

namespace eShop.WebApp.Helpers;

/// <summary>
/// تبدیل و قالب‌بندی تاریخ میلادی به هجری شمسی برای نمایش در UI.
/// توجه: تاریخ همچنان به‌صورت UTC/DateTime میلادی در دیتابیس ذخیره می‌شود؛
/// تنها لایهٔ نمایش تغییر می‌کند تا از خطای ذخیره‌سازی جلوگیری شود.
/// </summary>
public static class PersianDateHelper
{
    private static readonly PersianCalendar _pc = new();

    /// <summary>
    /// تاریخ میلادی را به رشتهٔ شمسی yyyy/MM/dd تبدیل می‌کند.
    /// </summary>
    public static string ToPersianDateString(this DateTime gregorianDate, string format = "yyyy/MM/dd")
    {
        // اگر تاریخ UTC است، به وقت ایران (Asia/Tehran) تبدیل می‌کنیم
        var local = gregorianDate.Kind == DateTimeKind.Utc
            ? TimeZoneInfo.ConvertTimeFromUtc(gregorianDate, GetTehranTimeZone())
            : gregorianDate;

        // استفاده مستقیم از PersianCalendar برای دقت
        var year = _pc.GetYear(local);
        var month = _pc.GetMonth(local);
        var day = _pc.GetDayOfMonth(local);

        return format switch
        {
            "yyyy/MM/dd" => $"{year:0000}/{month:00}/{day:00}",
            "yyyy-MM-dd" => $"{year:0000}-{month:00}-{day:00}",
            "short" => $"{year:0000}/{month:00}/{day:00}",
            "long" => $"{day} {GetPersianMonthName(month)} {year}",
            _ => $"{year:0000}/{month:00}/{day:00}"
        };
    }

    public static string ToPersianDateTimeString(this DateTime gregorianDate)
    {
        var local = gregorianDate.Kind == DateTimeKind.Utc
            ? TimeZoneInfo.ConvertTimeFromUtc(gregorianDate, GetTehranTimeZone())
            : gregorianDate;

        var year = _pc.GetYear(local);
        var month = _pc.GetMonth(local);
        var day = _pc.GetDayOfMonth(local);
        return $"{year:0000}/{month:00}/{day:00} – {local:HH:mm}";
    }

    public static string GetPersianMonthName(int month) => month switch
    {
        1 => "فروردین",
        2 => "اردیبهشت",
        3 => "خرداد",
        4 => "تیر",
        5 => "مرداد",
        6 => "شهریور",
        7 => "مهر",
        8 => "آبان",
        9 => "آذر",
        10 => "دی",
        11 => "بهمن",
        12 => "اسفند",
        _ => ""
    };

    private static TimeZoneInfo GetTehranTimeZone()
    {
        try
        {
            // لینوکس/داکر
            return TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
        }
        catch
        {
            try
            {
                // ویندوز
                return TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            }
            catch
            {
                return TimeZoneInfo.Utc;
            }
        }
    }
}
