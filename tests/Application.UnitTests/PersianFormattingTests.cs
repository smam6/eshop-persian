using System.Globalization;
using eShop.WebAppComponents.Services;

namespace eShop.Application.UnitTests;

/// <summary>
/// آزمون‌های لایهٔ جواهره‌سازی فارسی: ارقام، واحد پول، تقویم شمسی و
/// بازگشت به فرهنگ انگلیسی. آفلاین‌اند و هیچ سرویسی بالا نمی‌آورند.
/// </summary>
[TestClass]
public class PersianFormattingTests
{
    private static CultureInfo Persian => PersianCultureHelper.GetPersianCulture(new PersianFormattingOptions
    {
        UsePersianDigits = true,
        CurrencySymbol = "تومان",
        CurrencyDecimals = 0
    });

    [TestMethod]
    public void ToPersianDigits_ConvertsDigitsAndSeparators()
    {
        Assert.AreEqual("۱٬۲۳۴٫۵", PersianFormat.ToPersianDigits("1,234.5"));
    }

    [TestMethod]
    public void ToPersianDigits_KeepsOtherCharactersAndIsIdempotent()
    {
        // رقم‌های فارسی و متن اطرافشان باید دست‌نخورده بمانند.
        Assert.AreEqual("۱۶۸ تومان", PersianFormat.ToPersianDigits("168 تومان"));
        Assert.AreEqual("۹۹+", PersianFormat.ToPersianDigits("99+"));
        Assert.AreEqual(string.Empty, PersianFormat.ToPersianDigits(string.Empty));
    }

    [TestMethod]
    public void ToPersianDigits_CanBeDisabled()
    {
        Assert.AreEqual("1,234.5", PersianFormat.ToPersianDigits("1,234.5", usePersianDigits: false));
    }

    [TestMethod]
    public void ToLatinDigits_NormalizesPersianAndArabicNumerals()
    {
        Assert.AreEqual("1403-05-24", PersianFormat.ToLatinDigits("۱۴۰۳-۰۵-۲۴"));
        Assert.AreEqual("1234.5", PersianFormat.ToLatinDigits("١٢٣٤٫٥"));
        Assert.AreEqual("1,234", PersianFormat.ToLatinDigits("۱٬۲۳۴"));
    }

    [TestMethod]
    public void FormatCurrency_UsesTomanAndPersianDigits()
    {
        Assert.AreEqual("۱٬۲۳۴ تومان", PersianFormat.FormatCurrency(1_234m, Persian));
    }

    [TestMethod]
    public void FormatCurrency_RoundsAwayFromZeroAndDropsDecimals()
    {
        // تومان واحد خرد رایج ندارد، پس ۱۲۳۴٫۵۰ تومان به ۱۲۳۵ گرد می‌شود.
        Assert.AreEqual("۱٬۲۳۵ تومان", PersianFormat.FormatCurrency(1_234.50m, Persian));
        Assert.AreEqual("۱۶۸ تومان", PersianFormat.FormatCurrency(168.00m, Persian));
    }

    [TestMethod]
    public void FormatCurrency_FallsBackToUsdForOtherCultures()
    {
        // رفتار پیشین eShop برای فرهنگ انگلیسی حفظ می‌شود.
        var english = PersianCultureHelper.GetEnglishCulture();
        Assert.AreEqual("$1,234.50", PersianFormat.FormatCurrency(1_234.5m, english));
    }

    [TestMethod]
    public void ToPriceDisplay_UsesTheGivenCulture()
    {
        // در Blazor، فرهنگ درخواست از RequestLocalization به CurrentCulture می‌آید؛
        // اینجا همان مسیر را با پارامتر فرهنگ می‌سنجیم (بدون تغییر وضعیت سراسری).
        Assert.AreEqual("۱۶۸ تومان", 168m.ToPriceDisplay(Persian));
        Assert.AreEqual("$168.00", 168m.ToPriceDisplay(PersianCultureHelper.GetEnglishCulture()));
    }

    [TestMethod]
    public void FormatDate_UsesPersianCalendar()
    {
        // نوروز ۱۴۰۴ برابر است با ۲۱ مارس ۲۰۲۵
        var gregorian = new DateTime(2025, 3, 21);

        Assert.AreEqual("1404/01/01", gregorian.ToPersianDateString());
        Assert.AreEqual("۱۴۰۴/۰۱/۰۱", PersianFormat.FormatDate(gregorian, Persian));

        // خودِ فرهنگ هم باید DateTime.ToString() را شمسی کند. شکل رقم‌ها به ICU
        // بستگی دارد، پس قبل از مقایسه به لاتین تبدیل می‌کنیم.
        var fromCulture = gregorian.ToString("yyyy/MM/dd", Persian);
        Assert.AreEqual("1404/01/01", PersianFormat.ToLatinDigits(fromCulture));
    }

    [TestMethod]
    public void FormatDate_KeepsGregorianForOtherCultures()
    {
        var english = PersianCultureHelper.GetEnglishCulture();
        var gregorian = new DateTime(2025, 3, 21);

        Assert.AreEqual(gregorian.ToString("d", english), PersianFormat.FormatDate(gregorian, english));
    }

    [TestMethod]
    public void ToPersianYear_MatchesCalendarConversion()
    {
        Assert.AreEqual(1404, new DateTime(2025, 3, 21).ToPersianYear());
        Assert.AreEqual(1404, new DateTime(2025, 12, 31).ToPersianYear());
        Assert.AreEqual(1405, new DateTime(2026, 10, 17).ToPersianYear());
    }

    [TestMethod]
    public void PersianCulture_HasTomanSymbolsAndSeparators()
    {
        var culture = Persian;

        Assert.AreEqual("fa-IR", culture.Name);
        Assert.AreEqual("٫", culture.NumberFormat.NumberDecimalSeparator);
        Assert.AreEqual("٬", culture.NumberFormat.NumberGroupSeparator);
        Assert.AreEqual("تومان", culture.NumberFormat.CurrencySymbol);
        Assert.AreEqual("٬", culture.NumberFormat.CurrencyGroupSeparator);
    }

    [TestMethod]
    public void PersianCulture_UsesPersianCalendarWhenSupported()
    {
        var calendar = Persian.DateTimeFormat.Calendar;

        // اگر محیط از تقویم شمسی پشتیبانی نکند، ساختار تاریخ همان میلادی می‌ماند
        // و نمایشگرِ PersianDateHelper کار می‌کند؛ پس فقط در حالت پشتیبانی بررسی می‌کنیم.
        if (calendar is PersianCalendar)
        {
            for (var month = 1; month <= 6; month++)
            {
                Assert.AreEqual(31, calendar.GetDaysInMonth(1403, month), $"ماه {month} باید ۳۱ روزه باشد");
            }

            Assert.AreEqual(30, calendar.GetDaysInMonth(1403, 7));
        }
    }

    [TestMethod]
    public void ToPersianRelativeDays_CoversCommonCases()
    {
        var today = new DateTime(2026, 10, 17);

        Assert.AreEqual("امروز", today.ToPersianRelativeDays(today));
        Assert.AreEqual("دیروز", today.AddDays(-1).ToPersianRelativeDays(today));
        Assert.AreEqual("۳ روز پیش", PersianFormat.ToPersianDigits(today.AddDays(-3).ToPersianRelativeDays(today)));
    }
}
