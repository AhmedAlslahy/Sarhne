using Microsoft.EntityFrameworkCore;
using Sarhne.DAL.Database;

namespace Sarhne.API.Extensions;

public static class Database
{
    public static IServiceCollection AddDatabase(
   this IServiceCollection services,
   IConfiguration configuration)
    {
        services.AddDbContext<SarhneDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ProjectConnection")));

        return services;
    }
}