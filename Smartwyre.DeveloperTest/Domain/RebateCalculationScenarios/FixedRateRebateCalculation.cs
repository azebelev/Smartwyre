using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios.Abstracts;

namespace Smartwyre.DeveloperTest.Services.RebateService.CalculationScenarios;

public class FixedRateRebateCalculation : BaseCalculationScenario
{
    protected override SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedRateRebate;

    protected override decimal CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request) =>
        product.Price * rebate.Percentage * request.Volume;

    protected override bool Validate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate.Percentage == 0)
            ValidationsErrors.Add("The percentage should not be equal 0");

        if (product.Price == 0)
            ValidationsErrors.Add("The price should not be equal 0");

        if (request.Volume == 0)
            ValidationsErrors.Add("The request volume should not be equal 0");

        return ValidationsErrors.Count == 0;
    }
}