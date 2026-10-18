
namespace eShop.Identity.API;

public class UsersSeed(ILogger<UsersSeed> logger, UserManager<ApplicationUser> userManager) : IDbSeeder<ApplicationDbContext>
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        var alice = await userManager.FindByNameAsync("alice");

        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "آلیس اسمیت",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "تهران",
                Country = "ایران",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "اسمیت",
                Name = "آلیس",
                PhoneNumber = "09121234567",
                ZipCode = "15987",
                State = "تهران",
                Street = "خیابان آزادی، پلاک ۱۲۳",
                SecurityNumber = "123"
            };

            var result = await userManager.CreateAsync(alice, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("alice created");
            }
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("alice already exists");
            }
        }

        var bob = await userManager.FindByNameAsync("bob");

        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "باب اسمیت",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "تهران",
                Country = "ایران",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "اسمیت",
                Name = "باب",
                PhoneNumber = "09121234567",
                ZipCode = "15987",
                State = "تهران",
                Street = "خیابان آزادی، پلاک ۱۲۳",
                SecurityNumber = "456"
            };

            var result = await userManager.CreateAsync(bob, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("bob created");
            }
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("bob already exists");
            }
        }
    }
}
