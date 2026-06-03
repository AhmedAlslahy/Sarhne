using Microsoft.AspNetCore.Identity;

namespace Sarhne.API.Data.Seed
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
    public class RoleSeeder : IDataSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public RoleSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAsync()
        {
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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
}
