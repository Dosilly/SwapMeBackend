using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SwapMe.Application.Abstractions;
using SwapMe.Application.Config;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Application.Handlers.Authentication;
using SwapMe.Application.Handlers.Users;
using SwapMe.Infrastructure.Services;
using SwapMe.Infrastructure.Sql.Contexts;

namespace SwapMe.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersCommandHandler, UsersCommandHandler>()
            .AddScoped<IUsersService, UsersService>()
            .AddScoped<IAuthenticationRequestHandler, AuthenticationRequestHandler>();
        return services;
    }

    public static IServiceCollection ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("SwapMeUsers"),
            b => b.MigrationsAssembly(configuration.GetValue<string>("MigrationsAssembly"))));
        return services;
    }

    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
        return services;
    }

    public static IServiceCollection RegisterConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        return services;
    }
}