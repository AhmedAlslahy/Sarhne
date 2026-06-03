using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sarhne.API.Data.Seed;
using Sarhne.BLL.DTOs.Auth;
using Sarhne.BLL.Services.Implementation;
using Sarhne.BLL.Services.Interfaces;
using Sarhne.BLL.Validation.Auth;
using Sarhne.DAL.Database;
using Sarhne.DAL.Entities;
using Sarhne.DAL.Repository.Implementation;
using Sarhne.DAL.Repository.Interfaces;
using System.Text;


namespace Sarhne.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // don't remove this.
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            //-------------------------------------------------------------------------------------------------------
            var config = builder.Configuration.GetConnectionString("ProjectConnection");
            builder.Services.AddDbContext<SarhneDbContext>(options =>
                options.UseSqlServer(config));
            //---------------------------------------------------------------------------------------------------------

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<SarhneDbContext>();
            //---------------------------------------------------------------------------------------------

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                {
                    policy
                        .WithOrigins()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            //-----------------------------------------------------------------------------

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
             {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

              options.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidateIssuer = true,
                 ValidIssuer = builder.Configuration["JWTInformations:issuerIP"],

                 ValidateAudience = true,
                 ValidAudience = builder.Configuration["JWTInformations:audienceIP"],

                  ValidateIssuerSigningKey = true,
 
                   ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(builder.Configuration["JWTInformations:SecretKey"])
                     ),

                   ClockSkew = TimeSpan.Zero
                   };

                 options.Events = new JwtBearerEvents
                   {
                   OnMessageReceived = context =>
                     {
                       context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
         }
     };
 });

            //---------------------------------------------------------------------------------------------------------------

            // Add services to the container.

            //repository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
            builder.Services.AddScoped<IUserSettingRepo, UserSettingRepo>();
            builder.Services.AddScoped<IMessageRepo, MessageRepo>();

            //services
            builder.Services.Configure<JwtInformations>(builder.Configuration.GetSection("JWTInformations"));
            builder.Services.AddScoped<IUserSettingService, UserSettingService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            //validations
            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterDtoValidator).Assembly);
            //builder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = false;
            //});
            //------------------------------------------------------------------------------------------------------------------

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            //---------------------------------------------------------------------------------------------------------------------

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleSeeder = new RoleSeeder(services);
                await roleSeeder.SeedAsync();

                var adminSeeder = new AdminSeeder(services);
                await adminSeeder.SeedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}