using System.ComponentModel.DataAnnotations;

namespace eShop.WebApp.Services;

public class BasketCheckoutInfo
{
    [Required(ErrorMessage = "وارد کردن نشانی الزامی است.")]
    [Display(Name = "نشانی")]
    public string? Street { get; set; }

    [Required(ErrorMessage = "وارد کردن شهر الزامی است.")]
    [Display(Name = "شهر")]
    public string? City { get; set; }

    [Required(ErrorMessage = "وارد کردن استان الزامی است.")]
    [Display(Name = "استان")]
    public string? State { get; set; }

    [Required(ErrorMessage = "وارد کردن کشور الزامی است.")]
    [Display(Name = "کشور")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "وارد کردن کد پستی الزامی است.")]
    [Display(Name = "کد پستی")]
    public string? ZipCode { get; set; }

    public string? CardNumber { get; set; }

    public string? CardHolderName { get; set; }

    public string? CardSecurityNumber { get; set; }

    public DateTime? CardExpiration { get; set; }

    public int CardTypeId { get; set; }

    public string? Buyer { get; set; }
    public Guid RequestId { get; set; }
}
