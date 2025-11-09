

using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios.Abstracts;

namespace Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios;

public class AmountPerUomCalculation : BaseCalculationScenario
{
    protected override SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.AmountPerUom;

    protected override decimal CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Amount * request.Volume;

    protected override bool Validate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (product.Price == 0)
            ValidationsErrors.Add("The price should not be equal 0");

        if (request.Volume == 0)
            ValidationsErrors.Add("The request volume should not be equal 0");

        return ValidationsErrors.Count == 0;
    }
}