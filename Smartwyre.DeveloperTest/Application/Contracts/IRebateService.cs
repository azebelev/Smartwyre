using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;

namespace Smartwyre.DeveloperTest.Contracts;

public interface IRebateService
{
    CalculateRebateResult Calculate(CalculateRebateRequest request);
    bool IsVolumeRequiredForCalculation(CalculateRebateRequest request);
}
