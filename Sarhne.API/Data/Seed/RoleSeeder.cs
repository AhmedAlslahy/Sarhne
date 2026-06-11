using Sarhne.BLL.Helper;

namespace Sarhne.API.Data.Seed;

public interface IDataSeeder
{
    Task SeedAsync();
}

public class RoleSeeder(IServiceProvider serviceProvider) : IDataSeeder
{
    public async Task SeedAsync()
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await roleManager.CreateRoleAsync("Admin");
        await roleManager.CreateRoleAsync("User");
    }
}