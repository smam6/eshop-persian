using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;
using Basket.API.Resources;
using eShop.Ordering.API.Resources;
using eShop.WebApp.Services;
using IdentityServerHost.Quickstart.UI;

namespace eShop.Application.UnitTests;

/// <summary>
/// آزمون‌های پیام‌های بومی‌سازی‌شدهٔ لایهٔ سرویس‌ها: وضعیت‌های gRPC سبد خرید،
/// پیام‌های BadRequest/ProblemDetails سفارش‌ها، و راستی‌آزمایی DataAnnotations در
/// WebApp/Identity. کاملاً آفلاین‌اند — منابع جاسازی‌شده (satellite) و خودِ
/// اعتبارسنجی بررسی می‌شوند و هیچ سرویسی بالا نمی‌آید.
/// </summary>
[TestClass]
public class LocalizedApiMessagesTests
{
    private CultureInfo? _previousUiCulture;

    [TestInitialize]
    public void SetPersianCulture()
    {
        _previousUiCulture = CultureInfo.CurrentUICulture;
        CultureInfo.CurrentUICulture = new CultureInfo("fa-IR");
    }

    [TestCleanup]
    public void RestoreCulture()
    {
        if (_previousUiCulture is not null)
        {
            CultureInfo.CurrentUICulture = _previousUiCulture;
        }
    }

    [TestMethod]
    public void Basket_UnauthenticatedMessage_IsPersianInFaCulture()
    {
        var rm = new ResourceManager(typeof(BasketApiResources));
        var message = rm.GetString(nameof(BasketApiResources.NotAuthenticated), CultureInfo.CurrentUICulture);
        Assert.IsNotNull(message);
        Assert.IsTrue(message!.Contains("احراز هویت"), $"یافت نشد: {message}");
    }

    [TestMethod]
    public void Basket_BasketDoesNotExistMessage_FormatsBuyerId()
    {
        var rm = new ResourceManager(typeof(BasketApiResources));
        var template = rm.GetString(nameof(BasketApiResources.BasketDoesNotExist), CultureInfo.CurrentUICulture);
        var message = string.Format(template!, "alice");
        Assert.IsTrue(message.Contains("alice") && message.Contains("سبد"), $"یافت نشد: {message}");
    }

    [TestMethod]
    public void Basket_Resources_FallBackToEnglish_ForEnUsCulture()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var rm = new ResourceManager(typeof(BasketApiResources));
        Assert.AreEqual(
            "The caller is not authenticated.",
            rm.GetString(nameof(BasketApiResources.NotAuthenticated), CultureInfo.CurrentUICulture));
    }

    [TestMethod]
    public void Ordering_CancelAndShipFailureMessages_ArePersian()
    {
        var rm = new ResourceManager(typeof(OrderingApiResources));
        var cancel = rm.GetString(nameof(OrderingApiResources.CancelFailed), CultureInfo.CurrentUICulture);
        var ship = rm.GetString(nameof(OrderingApiResources.ShipFailed), CultureInfo.CurrentUICulture);
        var missing = rm.GetString(nameof(OrderingApiResources.RequestIdMissing), CultureInfo.CurrentUICulture);
        Assert.AreEqual("لغو سفارش پردازش نشد.", cancel);
        Assert.AreEqual("ارسال سفارش پردازش نشد.", ship);
        Assert.AreEqual("کد درخواست ارسال نشده است.", missing);
    }

    [TestMethod]
    public void CheckoutInfo_RequiredErrors_AreLocalizedPersian()
    {
        var info = new BasketCheckoutInfo { Buyer = "alice" };
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(info, new ValidationContext(info), results, validateAllProperties: true);

        Assert.IsFalse(isValid);
        Assert.IsTrue(results.Any(r => r.ErrorMessage == "خیابان الزامی است."),
            string.Join(" | ", results.Select(r => r.ErrorMessage)));
        Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Country")));
    }

    [TestMethod]
    public void LoginInput_RequiredErrors_AreLocalizedPersian()
    {
        var model = new LoginInputModel();
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

        Assert.IsFalse(isValid);
        Assert.IsTrue(results.Any(r => r.ErrorMessage == "نام کاربری الزامی است."),
            string.Join(" | ", results.Select(r => r.ErrorMessage)));
        Assert.IsTrue(results.Any(r => r.ErrorMessage == "گذرواژه الزامی است."),
            string.Join(" | ", results.Select(r => r.ErrorMessage)));
    }
}
