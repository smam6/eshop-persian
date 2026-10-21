namespace eShop.WebApp.Resources;

/// <summary>
/// کلاس نشانگر برای IStringLocalizer – فایل‌های resx هم‌نام با این کلاس هستند.
/// </summary>
public class SharedResources
{
    // کلیدهای پیام‌های راستی‌آزمایی/برچسب‌ها؛ از [Required(ErrorMessageResourceName = nameof(...))]
    // و [Display(Name = nameof(...))] در BasketCheckoutInfo استفاده می‌شوند.
    public const string Validation_StreetRequired = nameof(Validation_StreetRequired);
    public const string Validation_CityRequired = nameof(Validation_CityRequired);
    public const string Validation_StateRequired = nameof(Validation_StateRequired);
    public const string Validation_CountryRequired = nameof(Validation_CountryRequired);
    public const string Validation_ZipCodeRequired = nameof(Validation_ZipCodeRequired);
    public const string Label_Street = nameof(Label_Street);
    public const string Label_City = nameof(Label_City);
    public const string Label_State = nameof(Label_State);
    public const string Label_Country = nameof(Label_Country);
    public const string Label_ZipCode = nameof(Label_ZipCode);
}
