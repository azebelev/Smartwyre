using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Contracts;
using Smartwyre.DeveloperTest.Services.CommunicationService;
using Smartwyre.DeveloperTestDeveloperTest.Application.Services.RebateService;

namespace Smartwyre.DeveloperTest.Runner.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IRebateService, RebateService>();
        services.AddScoped<ICommunicationService, CommunicationService>();

        return services;
    }
}