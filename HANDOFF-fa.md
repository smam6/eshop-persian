# راهنمای تحویل — فارسی‌سازی eShop

این تغییرات دو فاصلهٔ آخر باقی‌مانده از بازبینی کامل فارسی‌سازی را می‌بندد:
بومی‌سازی پیام‌های خطای سرویس‌ها (وضعیت‌های gRPC سبد خرید و پیام‌های
BadRequest/ProblemDetails بخش سفارش‌ها، هم‌سان الگوی Catalog) و اتصال
راستی‌آزمایی DataAnnotations فرم‌ها (نشانی checkout در WebApp و فرم ورود
Identity) به فایل‌های منابع با provider استاندارد MVC. در کنار آن، ترجمهٔ
صفحات Grants/Diagnostics، تبدیل نشتی‌های LTR در `site.css` به ویژگی‌های
منطقی، رفع آدرس‌های پکیج در `package-lock.json`، بستهٔ صحت‌سنجی محلی
(`scripts/verify-persian-build.sh` و `tools/check-persian.py`) و آزمون‌های
جدید `LocalizedApiMessagesTests` افزوده شده‌اند.

## چگونه این تغییرات را اعمال کنید

اگر برنچ `persian-localization` روی گیت‌هاب موجود است (که با بازبینی این
تحویل ساخته شد)، همان PR را merge کنید. در غیر این صورت فایل patch را اعمال کنید:

```bash
git clone https://github.com/smam6/eshop-persian.git
cd eshop-persian
git checkout -b persian-localization
# سپس فایل patch را در ریشهٔ مخزن کپی کنید و اجرا کنید:
git am --3way 0001-feat-localization.patch
# یا اگر git am خطا داد:
git apply --3way 0001-feat-localization.patch
git add -A
git commit -m "feat: complete Persian localization and service error messages"
```

## چگونه تأیید کنید

پیش‌نیاز: دات‌نت SDK نسخهٔ ۱۰ و Docker.

```bash
dotnet build eShop.slnx -c Release
dotnet test tests/Application.UnitTests/Application.UnitTests.csproj -c Release
dotnet test tests/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj -c Release
dotnet test tests/ClientApp.UnitTests/ClientApp.UnitTests.sln -c Release
./scripts/verify-persian-build.sh
```

یا فقط اسکریپت یک‌پارچهٔ محلی:

```bash
./verify-locally.sh
```

## چک‌های نیمه‌تمام

بند‌های ۱–۱۵، ۱۸ (اجرای build)، ۲۹، ۳۱، ۳۲–۳۳ (اجرای runtime)، ۴۳–۴۸ در
محیطی که این تغییرات نوشته شد قابل اجرا نبودند (شبکهٔ سندباکس به
`dot.net`، `api.nuget.org` و `cdn.playwright.dev` دسترسی نداشت؛ خطای ثبت‌شده:
`curl: (35) SSL_ERROR_SYSCALL`). این بند‌ها باید روی ماشین شما با دات‌نت ۱۰ و
Docker اجرا شوند؛ جدول وضعیت کامل در `VERIFICATION-STATUS-fa.md`.

## پس از تأیید

```bash
git push -u origin persian-localization
gh pr create --base main --head persian-localization \
  --title "feat: Persian localization with service error localization" \
  --body "Closes remaining localization gaps: Basket/Ordering problem details in Persian, DataAnnotation messages in resource files, unit tests for localized messages. See HANDOFF-fa.md for verification steps."
gh pr merge --squash
```
