using System;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios;
using Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios.Abstracts;
using Smartwyre.DeveloperTest.Services.RebateService.CalculationScenarios;

namespace Smartwyre.DeveloperTest.Application.Factories;

public static class CalculationScenarioFactory
{
    public static BaseCalculationScenario Create(IncentiveType incentiveType)
    {
        return incentiveType switch
        {
            IncentiveType.FixedCashAmount => new FixedCashAmountCalculation(),
            IncentiveType.FixedRateRebate => new FixedRateRebateCalculation(),
            IncentiveType.AmountPerUom => new AmountPerUomCalculation(),
            _ => throw new NotSupportedException($"Unsupported incentive type: {incentiveType}")
        };
    }
}
