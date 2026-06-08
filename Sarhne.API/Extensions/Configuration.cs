namespace Sarhne.API.Extensions;

public static class Configuration
{
    public static WebApplicationBuilder AddProjectConfiguration(
        this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true)
            .AddEnvironmentVariables();

        return builder;
    }
}