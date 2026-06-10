namespace Sarhne.API.Data.Seed;

public class AdminSeeder(IServiceProvider serviceProvider) : IDataSeeder
{
    public async Task SeedAsync()
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var adminEmail = "admin@test.com";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new User
            {
                FullName = "admin",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                UserSetting = new UserSetting
                {
                    AllowAnonymousMessages = true,
                    ShowLastSeen = true,
                    ShowProfileViews = true
                }
            };

            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}