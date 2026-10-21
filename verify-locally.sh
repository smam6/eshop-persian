#!/usr/bin/env bash
# صحت‌سنجی یک‌پارچهٔ فارسی‌سازی — پیش‌نیاز: دات‌نت SDK ۱۰
set -u
cd "$(dirname "$0")"
LOG=.verify-logs; mkdir -p "$LOG"; : > "$LOG/results.txt"
FAILED=0
ok()  { printf 'PASS  %s\n' "$1" | tee -a "$LOG/results.txt"; }
bad() { printf 'FAIL  %s\n' "$1" | tee -a "$LOG/results.txt"; FAILED=1; }

V=$(dotnet --version 2>/dev/null) || { echo "dotnet not found"; exit 2; }
case "$V" in 10.*) ok "dotnet $V";; *) echo "dotnet $V found, 10.x required"; exit 2;; esac

run() { local n="$1"; shift; if "$@" >"$LOG/$n.log" 2>&1; then ok "$n"; else bad "$n (see $LOG/$n.log)"; fi; }
run restore dotnet restore eShop.slnx
run build   dotnet build eShop.slnx -c Release --no-restore
run unit    dotnet test tests/Application.UnitTests/Application.UnitTests.csproj -c Release --no-build
run catalog dotnet test tests/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj -c Release
run client  dotnet test tests/ClientApp.UnitTests/ClientApp.UnitTests.sln -c Release
run struct  python3 tools/check-persian.py

# اجرای WebApp و بررسی زندهٔ RTL/انگلیسی/فونت
(dotnet run --project src/WebApp/WebApp.csproj --launch-profile http >"$LOG/webapp.log" 2>&1) &
WPID=$!
sleep 20
BODY=$(curl -s http://localhost:5045/)
CODE=$(curl -s -o /dev/null -w '%{http_code}' http://localhost:5045/)
[ "$CODE" = "200" ] && printf '%s' "$BODY" | grep -q 'dir="rtl"' && ok "webapp rtl" || bad "webapp rtl"
curl -s 'http://localhost:5045/?culture=en-US' | grep -qi 'dir="ltr"\|lang="en"' && ok "en-US fallback" || bad "en-US fallback"
F=$(curl -s -o /dev/null -w '%{http_code}' http://localhost:5045/fonts/vazirmatn/Vazirmatn-Regular.woff2)
[ "$F" = "200" ] && ok "vazirmatn font" || bad "vazirmatn font (HTTP $F)"
kill $WPID 2>/dev/null; pkill -f "WebApp.dll" 2>/dev/null

echo "----------------------------------------"
if [ "$FAILED" -eq 0 ]; then echo "همهٔ بررسی‌ها سبز است."; else echo "برخی بررسی‌ها ناموفق — لاگ در $LOG/"; fi
exit $FAILED
