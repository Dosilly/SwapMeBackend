using SwapMe.Application.Abstractions;
using SwapMe.Application.Handlers.Abstractions;
using SwapMe.Application.Handlers.Users;
using SwapMe.Infrastructure.Services;

namespace SwapMe.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersCommandHandler, UsersCommandHandler>();
        services.AddScoped<IUsersService, UsersService>();
        return services;
    }
}