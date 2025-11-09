using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Contracts;
using Smartwyre.DeveloperTest.Runner.Extensions;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddPersistenceServices();
        services.AddApplicationServices();
        
        var serviceProvider = services.BuildServiceProvider();

        var communicationService = serviceProvider.GetRequiredService<ICommunicationService>();

        communicationService.Communicate();
    }
}
