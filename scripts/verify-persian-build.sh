#!/usr/bin/env bash
# بستهٔ صحت‌سنجی محلی فارسی‌سازی — روی ماشین توسعه‌دهنده (دات‌نت ۱۰ + Docker) اجرا شود.
# خروجی: جدول PASS/FAIL + لاگ کامل در .verify-logs/
set -u
cd "$(dirname "$0")/.."
LOGDIR=".verify-logs"; mkdir -p "$LOGDIR"; : > "$LOGDIR/summary.txt"
RESULT=0

run() { # run "نام بررسی" "کلمهٔ کلیدی موفقیت" cmd args...
  local name="$1" key="$2"; shift 2
  local log="$LOGDIR/$(echo "$name" | tr ' /:' '___').log"
  if "$@" > "$log" 2>&1 && { [ -z "$key" ] || grep -q "$key" "$log"; }; then
    printf 'PASS  %s\n' "$name" | tee -a "$LOGDIR/summary.txt"
  else
    printf 'FAIL  %s (لاگ: %s)\n' "$name" "$log" | tee -a "$LOGDIR/summary.txt"; RESULT=1
  fi
}

command -v dotnet >/dev/null || { echo "dotnet نصب نیست — نصب از https://dot.net"; exit 2; }
run "dotnet version" "" dotnet --version
run "structural checks" "سبز" python3 tools/check-persian.py
run "restore" "" dotnet restore eShop.slnx
run "build (Debug)" "" dotnet build eShop.slnx -c Debug --no-restore
run "unit tests" "" dotnet test tests/Application.UnitTests/Application.UnitTests.csproj -c Debug --no-build
run "catalog functional tests" "" dotnet test tests/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj -c Debug

echo "--- بررسی دستی پس از بالا آمدن برنامه (aspire run یا docker):"
echo "  * /  باید dir=\"rtl\" و فارسی باشد؛  /?culture=en-US  برگشت کامل به انگلیسی"
echo "  * ProblemDetails کاتالوگ/سفارش با فرهنگ fa-IR فارسی است"
echo "  * فونت وزیرمتن از DevTools مرورگر لود شده باشد"

exit $RESULT
