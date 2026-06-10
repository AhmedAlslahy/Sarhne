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

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}