using eShop.ClientApp.Models.Catalog;

namespace eShop.ClientApp.Services.Catalog;

public class CatalogMockService : ICatalogService
{
    private static readonly List<CatalogBrand> MockCatalogBrands =
        new() {new CatalogBrand {Id = 1, Brand = "Azure"}, new CatalogBrand {Id = 2, Brand = "Visual Studio"}};

    private static readonly List<CatalogType> MockCatalogTypes =
        new() {new CatalogType {Id = 1, Type = "ماگ"}, new CatalogType {Id = 2, Type = "تی‌شرت"}};

    private static readonly List<CatalogItem> MockCatalog =
        new()
        {
            new CatalogItem
            {
                Id = Common.Common.MockCatalogItemId01,
                PictureUri = "fake_product_01.png",
                Name = "ساعت مچی جی‌پی‌اس ادونچرر",
                Price = 20000000M,
                CatalogBrandId = 2,
                CatalogBrand = MockCatalogBrands[1],
                CatalogTypeId = 2,
                CatalogType = MockCatalogTypes[1],
                Description = "با اعتمادبه‌نفس مسیرتان را پیدا کنید؛ ساعت مچی ادونچرر بدنه‌ای مقاوم دارد و جی‌پی‌اس داخلی، قطب‌نما و ارتفاع‌سنج آن در هر زمینی همراهتان است. طراحی مشکی و خوانای آن، زیبایی و کارایی را با هم دارد."
            },
            new CatalogItem
            {
                Id = Common.Common.MockCatalogItemId02,
                PictureUri = "fake_product_02.png",
                Name = "کلاه ایمنی دوچرخه‌سواری ایرولایت",
                Price = 13000000M,
                CatalogBrandId = 2,
                CatalogBrand = MockCatalogBrands[1],
                CatalogTypeId = 2,
                CatalogType = MockCatalogTypes[1],
                Description = "در ماجراجویی‌های دوچرخه‌سواری ایمن بمانید؛ کلاه ایرولایت سبک و مقاوم است و سیستم تنظیم اندازه و تهویهٔ آن راحتی را فراهم می‌کند. رنگ سبز و طراحی سادهٔ آن در جاده چشمگیر است و برای دوچرخه‌سواری کوهستان و جاده مناسب است."
            },
            new CatalogItem
            {
                Id = Common.Common.MockCatalogItemId03,
                PictureUri = "fake_product_03.png",
                Name = "کوله‌پشتی آلپاین‌پک",
                Price = 12900000M,
                CatalogBrandId = 2,
                CatalogBrand = MockCatalogBrands[1],
                CatalogTypeId = 2,
                CatalogType = MockCatalogTypes[1],
                Description = "کوله‌پشتی آلپاین‌پک همراه همه‌فن شما در فضای باز است؛ طراحی سرمه‌ای با بندهای تقویت‌شده، ظرفیت ۴۵ لیتر، چندین بخش جداگانه و جای مخصوص قمقمه آب دارد. پشتی ارگونومیک آن حتی در سخت‌ترین مسیرها راحتی را حفظ می‌کند."
            },
            new CatalogItem
            {
                Id = Common.Common.MockCatalogItemId04,
                PictureUri = "fake_product_04.png",
                Name = "عینک اسکی فیوژن",
                Price = 8000000M,
                CatalogBrandId = 2,
                CatalogBrand = MockCatalogBrands[1],
                CatalogTypeId = 1,
                CatalogType = MockCatalogTypes[0],
                Description = "تجربهٔ اسکی خود را با عینک فیوژن کامل‌تر کنید؛ محافظت کامل در برابر اشعهٔ فرابنفش و عدسی‌های ضدبخار، دید شما را در شیب باز نگه می‌دارد. فریم نقره‌ای و عدسی نارنجی‌اش با بند تنظیم‌شونده و فوم نرم، راحتی یک‌روزه می‌سازد."
            },
            new CatalogItem
            {
                Id = Common.Common.MockCatalogItemId05,
                PictureUri = "fake_product_05.png",
                Name = "کاپشن پر آلپاین‌پیک",
                Price = 25000000M,
                CatalogBrandId = 1,
                CatalogBrand = MockCatalogBrands[0],
                CatalogTypeId = 2,
                CatalogType = MockCatalogTypes[1],
                Description = "کاپشن پر آلپاین‌پیک برای سرمای طاقت‌فرسا ساخته شده است؛ رنگ قرمز جسورانه و طراحی ساده‌اش، سبک و کارایی را به هم می‌رساند. عایق پر غاز باکیفیت، گرمای بی‌نظیری می‌دهد و کلاه جداشدنی، سرآستین تنظیم‌شونده و چند جیب زیپ‌دار دارد."
            }
        };

    public async Task<IEnumerable<CatalogItem>> GetCatalogAsync()
    {
        await Task.Delay(10);

        return MockCatalog;
    }

    public async Task<CatalogItem> GetCatalogItemAsync(int catalogItemId)
    {
        await Task.Delay(10);

        return MockCatalog.FirstOrDefault(x => x.Id == catalogItemId);
    }

    public async Task<IEnumerable<CatalogItem>> FilterAsync(int catalogBrandId, int catalogTypeId)
    {
        await Task.Delay(10);

        return MockCatalog
            .Where(
                c => c.CatalogBrandId == catalogBrandId &&
                     c.CatalogTypeId == catalogTypeId)
            .ToArray();
    }

    public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandAsync()
    {
        await Task.Delay(10);

        return MockCatalogBrands;
    }

    public async Task<IEnumerable<CatalogType>> GetCatalogTypeAsync()
    {
        await Task.Delay(10);

        return MockCatalogTypes;
    }
}
