using System.ComponentModel.DataAnnotations;
using eShop.WebApp.Resources;

namespace eShop.WebApp.Services;

public class BasketCheckoutInfo
{
    // راستی‌آزمایی/برچسب‌ها از منابع SharedResources (neutral + fa-IR) خوانده می‌شوند
    // تا با فرهنگ en-US هم پیام درست (انگلیسی) برگردد.
    [Display(ResourceType = typeof(SharedResources), Name = nameof(SharedResources.Label_Street))]
    [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.Validation_StreetRequired))]
    public string? Street { get; set; }

    [Display(ResourceType = typeof(SharedResources), Name = nameof(SharedResources.Label_City))]
    [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.Validation_CityRequired))]
    public string? City { get; set; }

    [Display(ResourceType = typeof(SharedResources), Name = nameof(SharedResources.Label_State))]
    [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.Validation_StateRequired))]
    public string? State { get; set; }

    [Display(ResourceType = typeof(SharedResources), Name = nameof(SharedResources.Label_Country))]
    [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.Validation_CountryRequired))]
    public string? Country { get; set; }

    [Display(ResourceType = typeof(SharedResources), Name = nameof(SharedResources.Label_ZipCode))]
    [Required(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.Validation_ZipCodeRequired))]
    public string? ZipCode { get; set; }

    public string? CardNumber { get; set; }

    public string? CardHolderName { get; set; }

    public string? CardSecurityNumber { get; set; }

    public DateTime? CardExpiration { get; set; }

    public int CardTypeId { get; set; }

    public string? Buyer { get; set; }
    public Guid RequestId { get; set; }
}
