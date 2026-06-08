using Sarhne.API.Extensions;
using Sarhne.API.Data.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.AddProjectConfiguration();
builder.Services.AddProjectServices(builder.Configuration);

//------------------------------------------------------------------------------------------

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await new RoleSeeder(services).SeedAsync();
    await new AdminSeeder(services).SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();