#!/usr/bin/env python3
"""بررسی‌های ساختاری محلی‌سازی فارسی — بدون نیاز به SDK.

اجرا:  python3 tools/check-persian.py [ریشهٔ مخزن]
خروجی: PASS/FAIL برای هر بند + کد خروج ۰ در حالت سبز.
"""
import json
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(sys.argv[1]) if len(sys.argv) > 1 else Path(__file__).resolve().parent.parent
fails: list[str] = []


def report(name: str, ok: bool, detail: str = "") -> None:
    print(f"{'✔' if ok else '✘'} {name}" + (f" — {detail}" if detail else ""))
    if not ok:
        fails.append(name)


# ۱) تقارن کلیدهای resx در همهٔ جفت‌های (neutral ↔ fa-IR) مخزن
all_en: set[str] = set()
for fp in sorted(ROOT.glob("src/**/*.fa-IR.resx")):
    base = fp.with_name(fp.name[: -len(".fa-IR.resx")] + ".resx")
    fa_keys = {e.get("name") for e in ET.parse(fp).getroot().findall("data")}
    en_keys = {e.get("name") for e in ET.parse(base).getroot().findall("data")} if base.exists() else set()
    all_en |= en_keys
    report(f"resx تقارن {base.parent.parent.name}/{base.name} (en={len(en_keys)} fa={len(fa_keys)})",
           base.exists() and en_keys == fa_keys, str(en_keys ^ fa_keys) if en_keys ^ fa_keys else "")

# ۲) کلیدهای مصرفی (L["k"] در razor/cshtml و localizer["k"] در C#) باید در برخی resx باشند
used = set()
for rp in list(ROOT.glob("src/**/*.razor")) + list(ROOT.glob("src/**/*.cshtml")) + list(ROOT.glob("src/**/*.cs")):
    txt = rp.read_text(encoding="utf-8-sig", errors="replace")
    used |= set(re.findall(r'L\["([^"]+)"\]', txt))
    used |= set(re.findall(r'localizer\["([^"]+)"\]', txt))
    used |= set(re.findall(r'localizer\[nameof\((?:\w+\.)+(\w+)\)\]', txt))
    used |= set(re.findall(r'(?:ErrorMessageResourceName|Name)\s*=\s*nameof\((?:\w+\.)+(\w+)\)\]', txt))
report(f"همهٔ {len(used)} کلید مصرفی در resx‌ها وجود دارند", used <= all_en, str(used - all_en) if used - all_en else "")

# ۳) schema دادهٔ نمونهٔ کاتالوگ + قانون قیمت تومان
items = json.loads((ROOT / "src/Catalog.API/Setup/catalog.json").read_text(encoding="utf-8"))
schema_ok = all(set(x) == {"Id", "Type", "Brand", "Name", "Description", "Price"} for x in items)
report("catalog.json: ساختار CatalogSourceEntry و ۱۰۱ آیتم", schema_ok and len(items) == 101, f"{len(items)} آیتم")
report("catalog.json: همهٔ قیمت‌ها مثبت و مضرب ۱۰۰٬۰۰۰ (تومان)",
       all(x["Price"] > 0 and x["Price"] % 100_000 == 0 for x in items))
ids = [x["Id"] for x in items]
report("catalog.json: Idها ۱..۱۰۱ بدون تغییر (تصاویر {Id}.webp معتبر)", ids == list(range(1, len(ids) + 1)))

# ۴) صفت‌های Razor: الگوی خراب `)""` و `"@L[` نباید باشد
bad = [str(p.relative_to(ROOT)) for p in list(ROOT.glob("src/**/*.razor")) + list(ROOT.glob("src/**/*.cshtml"))
       if ')""' in (t := p.read_text(encoding="utf-8-sig", errors="replace")) or '"@L[' in t]
report('صفت‌های Razor: نه `)""` و نه `"@L[`', not bad, str(bad))

# ۵) هر کامپوننت دارای L[" باید @inject IStringLocalizer داشته باشد
missing_inject = []
for p in ROOT.glob("src/**/*.razor"):
    t = p.read_text(encoding="utf-8-sig", errors="replace")
    if re.search(r'\bL\["', t) and "IStringLocalizer" not in t:
        missing_inject.append(str(p.relative_to(ROOT)))
report("هر کامپوننتِ مصرف‌کنندهٔ L، @inject IStringLocalizer دارد", not missing_inject, str(missing_inject))

# ۶) ریشه‌های HTML با dir="rtl" و FlowDirection مایو
rtl_targets = ["src/WebApp/Components/App.razor", "src/WebhookClient/Components/App.razor",
               "src/Identity.API/Views/Shared/_Layout.cshtml", "src/HybridApp/wwwroot/index.html"]
rtl_hits = [d for d in rtl_targets if (ROOT / d).exists() and 'dir="rtl"' in (ROOT / d).read_text(encoding="utf-8-sig")]
shell = ROOT / "src/ClientApp/AppShell.xaml"
maui_flow = shell.exists() and 'FlowDirection="RightToLeft"' in shell.read_text(encoding="utf-8")
report('ریشهٔ RTL در WebApp/Hybrid/WebhookClient/Identity + FlowDirection مایو',
       len(rtl_hits) == 4 and maui_flow, f"{len(rtl_hits)}/4 html" + ("" if maui_flow else " — AppShell.xaml بدون FlowDirection"))

# ۷) فونت وزیرمتن در پروژه‌های دارای رابط گرافیکی
FONTDIRS = ["src/WebApp/wwwroot/fonts/vazirmatn", "src/HybridApp/wwwroot/fonts/vazirmatn",
            "src/WebhookClient/wwwroot/fonts/vazirmatn", "src/Identity.API/wwwroot/fonts/vazirmatn"]
fonts = [d for d in FONTDIRS if len(list((ROOT / d).glob("*.woff2"))) >= 4]
report("فونت وزیرمتن (۴ وزن) در ۴ پروژه", len(fonts) == 4, f"{len(fonts)}/4")

print()
if fails:
    print(f"❌ {len(fails)} بررسی ناموفق.")
    sys.exit(1)
print("✅ همهٔ بررسی‌های ساختاری سبز است. برای build/test واقعی: ./scripts/verify-persian-build.sh")
