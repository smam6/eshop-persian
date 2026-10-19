using System.Globalization;

namespace eShop.WebAppComponents.Services;

/// <summary>
/// قالب‌بندی قیمت برای نمایش در رابط کاربری.
/// در فرهنگ فارسی مبلغ به <c>تومان</c> با ارقام فارسی و جداکنندهٔ هزارگان
/// «٬» و بدون رقم اعشار، و در فرهنگ‌های دیگر همان رفتار پیشین eShop
/// (دلار با دو رقم اعشار) برگردانده می‌شود.
/// </summary>
public static class PriceFormatter
{
    /// <summary>
    /// نمایش قیمت. نماد واحد پول داخل همین خروجی است؛ پس در .razor پیشوند
    /// <c>$</c> جداگانه نوشته نمی‌شود.
    /// </summary>
    public static string ToPriceDisplay(this decimal value)
        => value.ToPriceDisplay(CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToPriceDisplay(decimal)"/>
    public static string ToPriceDisplay(this decimal value, CultureInfo culture)
        => PersianFormat.FormatCurrency(value, culture);
}
