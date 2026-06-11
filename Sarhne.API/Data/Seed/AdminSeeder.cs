using Sarhne.BLL.Helper;

namespace Sarhne.API.Data.Seed;

public class AdminSeeder(IServiceProvider serviceProvider) : IDataSeeder
{
    public async Task SeedAsync()
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        await userManager.CreateAdminAsync("Admin", "admin@test.com", "Admin@123");
        await userManager.CreateAdminAsync("Admin1", "admin1@test.com", "Admin@123");
    }
}