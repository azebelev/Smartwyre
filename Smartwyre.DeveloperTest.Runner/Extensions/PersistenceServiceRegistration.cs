using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Persistence;

namespace Smartwyre.DeveloperTest.Runner.Extensions;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {

        services.AddScoped<IRebateRepository, RebateRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IRebateCalculationRepository, RebateCalculationRepository>();

        return services;
    }
}