namespace Basket.API.Resources;

/// <summary>
/// کلاس نشانگر (marker) برای IStringLocalizer&lt;BasketApiResources&gt;؛ فایل‌های resx
/// هم‌نام این کلاس در پوشهٔ Resources قرار می‌گیرند تا نام منبعِ تولیدشده با
/// انتظارِ کارخانهٔ منابع (RootNamespace = Basket.API) مطابقت کند.
/// </summary>
public sealed class BasketApiResources
{
    public const string NotAuthenticated = nameof(NotAuthenticated);
    public const string BasketDoesNotExist = nameof(BasketDoesNotExist);
}
