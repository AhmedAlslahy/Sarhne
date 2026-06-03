
using Microsoft.AspNetCore.Identity;
using Sarhne.DAL.Entities;

namespace Sarhne.API.Data.Seed
{
    public class AdminSeeder : IDataSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public AdminSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAsync()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

            var adminEmail = "admin@test.com";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
