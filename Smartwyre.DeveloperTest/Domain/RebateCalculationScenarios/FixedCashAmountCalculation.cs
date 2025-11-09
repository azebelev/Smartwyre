using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios.Abstracts;

namespace Smartwyre.DeveloperTest.Services.RebateService.CalculationScenarios;

public class FixedCashAmountCalculation : BaseCalculationScenario
{
    protected override SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedCashAmount;

    protected override decimal CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request) => rebate.Amount;

    protected override bool Validate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (rebate.Amount == 0)
        {
            ValidationsErrors.Add("The amount is equal 0");
            return false;
        }

        return true;
    }
}