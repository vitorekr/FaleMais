using Microsoft.Extensions.DependencyInjection;
using FaleMais.Application.Interfaces;
using FaleMais.Application.Services;

namespace FaleMais.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICallCostService, CallCostService>();
        return services;
    }
}
