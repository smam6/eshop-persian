# برنامهٔ مرجع eShop — نسخهٔ فارسی «AdventureWorks»

<div dir="rtl">

این مخزن نسخهٔ بومی‌سازی‌شدهٔ [برنامهٔ مرجع dotnet/eShop](https://github.com/dotnet/eShop)
است: یک فروشگاه اینترنتی با معماری خدمات‌محور روی [.NET 10 و Aspire](https://aspire.dev/)
که به‌طور کامل **فارسی و راست‌به‌چپ** شده است:

- 🌐 رابط کاربری فارسی روان در WebApp، HybridApp، WebhookClient، Identity و اپ موبایل MAUI
- ↔️ چیدمان RTL (جا به‌جای منطقی در CSS، `dir="rtl"`، `FlowDirection` در MAUI) + فونت وزیرمتن
- 📅 تاریخ‌ها با تقویم هجری شمسی و منطقهٔ زمانی تهران
- 🔢 ارقام فارسی و قیمت‌گذاری با واحد **تومان** (تبدیل ۱۰۰٬۰۰۰× دلار در دادهٔ نمونه)
- 🗂️ دادهٔ نمونهٔ کاتالوگ (۱۰۱ کالا) و کاربران نمایشی به فارسی
- 🧪 سازگاری آزمون‌های واحد، کارکردی و سرتاسری (xUnit + Playwright) با رابط فارسی

> فارسی حالت پیش‌فرض است؛ با `?culture=en-US` می‌توانید متن‌های resx را به
> انگلیسی برگردانید. سند کامل گام‌ها در پوشهٔ [`docs/`](docs) است.

</div>

## پیش‌نیازها

1. نصب [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) مطابق `global.json`.
2. نصب [Aspire CLI](https://aspire.dev/get-started/install-cli/) (`aspire --version`).
3. یک زمان‌بند کانتینر OCI مثل Docker Desktop (برای Redis و Postgres).
4. برای آزمون‌های سرتاسری: Node.js + `npm ci && npx playwright install chromium`.

## اجرای سریع

```console
git clone https://github.com/smam6/eshop-persian.git
cd eshop-persian
dotnet restore
aspire run
```

سپس پیوند برنامهٔ `webapp` (پورت پیش‌فرض 5045) را باز کنید. صفحهٔ خانه،
سبد خرید، تسویه حساب و «سفارش‌های من» همگی فارسی، راست‌به‌چپ و با تاریخ شمسی‌اند.

### کاربران نمایشی

| کاربر | گذرواژه | توضیح |
|---|---|---|
| `alice` | `Pass123$` | نشانی نمونهٔ تهران از پیش پر شده |
| `bob` | `Pass123$` | همان نشانی برای تست سفارش‌های موازی |

### تنظیمات فارسی

در `src/WebApp/appsettings.json` (و بخش هم‌نام در سایر پروژه‌ها):

```json
"Persian": {
  "UsePersianDigits": true,
  "CurrencySymbol": "تومان",
  "CurrencyDecimals": 0,
  "ShowCurrencySymbol": true
}
```

## آزمون‌ها

```console
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj
dotnet test tests/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj
dotnet test tests/ClientApp.UnitTests/ClientApp.UnitTests.sln
npm run test:e2e
```

جزئیات و نتایج مورد انتظار در [`docs/05-راهنمای-اجرا-و-اعتبارسنجی.md`](docs/05-راهنمای-اجرا-و-اعتبارسنجی.md).

## ساختار پروژه

۱۹ پروژه (۵ API: Catalog/Basket/Identity/Ordering/Webhooks، کلاینت‌های
WebApp/HybridApp/ClientApp/WebhookClient، لایهٔ مشترک WebAppComponents و
آزمون‌ها) — نقشهٔ کامل و جدول وضعیت هر پروژه در
[`docs/00-بررسی-ساختار-پروژه.md`](docs/00-بررسی-ساختار-پروژه.md).

مستندات گام‌به‌گام:

1. [بررسی ساختار](docs/00-بررسی-ساختار-پروژه.md)
2. [محلی‌سازی سمت سرور](docs/01-محلی‌سازی-سمت-سرور.md)
3. [چیدمان راست‌به‌چپ](docs/02-چیدمان-راست‌به‌چپ.md)
4. [تقویم شمسی و قالب‌بندی اعداد](docs/03-تقویم-شمسی-و-قالب‌بندی-اعداد.md)
5. [ترجمهٔ محتوا و دادهٔ نمونه](docs/04-ترجمه-محتوا-و-داده-نمونه.md)
6. [راهنمای اجرا و اعتبارسنجی](docs/05-راهنمای-اجرا-و-اعتبارسنجی.md)

## محدودیت‌های شناخته‌شده

- کامپایل/اجرای آزمون‌ها در محیطی که این تغییرات نوشته شده انجام نشده (نبود
  SDK دات‌نت)؛ پیش از ادغام، `dotnet build && dotnet test` را اجرا کنید.
- Bootstrap نسخهٔ LTR است و لایهٔ RTL به‌صورت فایل کمکی `rtl-fa.css` نوشته شده.
- ارقام فارسی فقط در لایهٔ نمایش است؛ ورودی‌های عددی/متنی لاتین می‌مانند تا
  parsing مرورگر نشکند.
- صفحات اشکال‌زدای Identity (Diagnostics/Grants) و رشته‌های لاگ انگلیسی‌اند.

---

این پروژه بر پایهٔ مجوز [MIT](LICENSE) همانند مخزن اصلی dotnet/eShop منتشر
می‌شود. فونت وزیرمتن زیرمجموعهٔ مجوز OFL است
(`src/WebApp/wwwroot/fonts/vazirmatn/OFL.txt`).
