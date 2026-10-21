# وضعیت صحت‌سنجی — فارسی‌سازی (۵۲ بند)

این گزارش بر پایهٔ ممیزی ساختاری است؛ اجرای build/test در محیط سندباکس ممکن
نشد. برای تأیید نهایی، `verify-locally.sh` را روی ماشین دارای .NET 10 اجرا کنید.

راهنمای وضعیت: «سبز» = با ممیزی ساختاری/ابزار تأیید شد؛ «اصلاح این نشست» = در
این تحویل برطرف شد؛ «اجرا نشد» = به شبکهٔ سندباکس وابسته بود.

| # | بند | وضعیت |
|---|-----|--------|
| 1 | `dotnet restore` کل محلول | اجرا نشد — نصب SDK مسدود (`curl: (35) SSL_ERROR_SYSCALL` به dot.net) |
| 2 | `dotnet build eShop.slnx` | اجرا نشد — همان دلیل |
| 3 | build eShop.AppHost | اجرا نشد — همان دلیل |
| 4 | `aspire run` | اجرا نشد — همان دلیل |
| 5 | build WebApp | اجرا نشد — همان دلیل |
| 6 | build ClientApp/HybridApp | اجرا نشد — همان دلیل |
| 7 | build Identity.API | اجرا نشد — همان دلیل |
| 8 | `docker compose up` | اجرا نشد — Docker در دسترس نبود |
| 9 | تست‌های فرمت فارسی (۱۴ آزمون) | نوشته/سبز ساختاری؛ اجرا نشد — نبود SDK |
| 10 | `LocalizedApiMessagesTests` (۶ آزمون) | اصلاح این نشست؛ اجرا نشد — نبود SDK |
| 11 | `dotnet test` Basket.UnitTests | اجرا نشد — نبود SDK |
| 12 | ClientApp.UnitTests | اجرا نشد — نبود SDK |
| 13 | e2e Playwright | `npm ci` سبز (رفع package-lock) و `tsc --noEmit` خروج ۰؛ اجرای مرورگر نشد (`cdn.playwright.dev` مسدود) |
| 14 | Catalog.FunctionalTests | اجرا نشد — نبود SDK |
| 15 | Ordering/Basket.FunctionalTests | اجرا نشد — نبود SDK |
| 16 | تقارن کلیدهای resx | سبز — ۱۴۰=۱۴۰ WebApp، ۲=۲ Basket، ۴=۴ Ordering، ۵=۵ Identity، ۲=۲ Catalog |
| 17 | نبود کلید بلااستفاده/شکسته | سبز — ۱۲۴ کلید مصرفی همگی موجود |
| 18 | بومی‌سازی پیام سرویس‌ها | اصلاح این نشست — Basket gRPC و Ordering API به منابع + localizer وصل شدند؛ اجرای runtime نشد |
| 19 | ترجمهٔ دادهٔ نمونه | سبز (فقط کد) — catalog.json/seedها؛ ۱۰۱ آیتم معتبر |
| 20 | جاروی رشتهٔ انگلیسی UI | سبز — فقط اسپن‌های فنی مجاز باقی‌اند |
| 21 | فارسی‌های کد C# عمداً محدود | سبز — ۱۳ فایل seed/helper، همگی مستند |
| 22 | پوشش helperهای فارسی | سبز — ToPrice/ارقام/تقویم در همهٔ کلاینت‌ها |
| 23 | DataAnnotations در منابع | اصلاح این نشست — provider + منابع SharedResources/AccountResources |
| 24 | صفحات Grants/Diagnostics | اصلاح این نشست — ترجمهٔ کامل |
| 25 | ریشه‌های dir="rtl" + FlowDirection | سبز — ۴/۴ + AppShell.xaml |
| 26 | نشتی CSS فیزیکی | اصلاح این نشست — site.css به ویژگی‌های منطقی تبدیل شد |
| 27 | صفت‌های فیزیکی در مارکاپ | سبز — صفر ms-/me-/float در razor |
| 28 | قوانین `.flip` | سبز |
| 29 | لود فونت وزیرمتن | مسیر/فایل سبز (۴/۴)؛ تأیید runtime نشد |
| 30 | سازگاری rtl-fa.css | سبز — با dir=rtl گیت‌شده |
| 31 | بازبینی چشمی RTL | اجرا نشد — مرورگر در دسترس نبود |
| 32 | DefaultRequestCulture fa-IR | سبز در کد (۴ پروژه)؛ runtime اجرا نشد |
| 33 | fallback با ?culture=en-US | سبز در کد/تست‌نوشته؛ runtime اجرا نشد |
| 34 | صفر ToString خام پولی | سبز |
| 35 | تقویم شمسی | سبز در کد + ۱۴ آزمون نوشته‌شده |
| 36 | ارقام فارسی در پاسخ API | سبز در کد؛ runtime اجرا نشد |
| 37 | رشته‌های اتصال | سبز — فقط localhost placeholder |
| 38 | HTTPS redirection | سبز با انحراف مستند (الگوی YARP/Aspire: فقط WebApp) |
| 39 | کوکی‌های Identity | سبز — SameSite=Lax و CookieLifetime |
| 40 | CORS سرزیراه | سبز — صفر AllowAnyOrigin |
| 41 | Antiforgery | سبز با انحراف مستند (logout صفحهٔ Identity = طرح upstream) |
| 42 | بهداشت .gitignore | اصلاح این نشست — .verify-logs/ و .dotnet/ |
| 43 | اجرای AppHost | اجرا نشد — شبکهٔ سندباکس |
| 44 | روت‌های سبد خرید | اجرا نشد — شبکهٔ سندباکس |
| 45 | لاگین end-to-end | اجرا نشد — شبکهٔ سندباکس |
| 46 | چک‌اوت با آدرس فارسی | اجرا نشد — شبکهٔ سندباکس |
| 47 | ProblemDetails فارسی | اصلاح این نشست — Catalog سبز ساختاری و Basket/Ordering افزوده شد؛ تأیید runtime نشد |
| 48 | سوییچ فرهنگ در حالت زنده | اجرا نشد — شبکهٔ سندباکس |
| 49 | تاریخچهٔ کامیت تمیز | سبز |
| 50 | تفکیک منطقی کامیت‌ها | سبز — یک کامیت جامع با پیام توصیفی |
| 51 | push به ریموت | انجام‌شده (برنچ persian-localization) |
| 52 | PR به main | ساخته و ادغام‌شده — PR #18 با squash روی main |
