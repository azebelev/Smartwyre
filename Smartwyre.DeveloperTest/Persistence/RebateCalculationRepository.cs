using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Data;

public class RebateCalculationRepository : IRebateCalculationRepository
{
    private readonly Dictionary<int, RebateCalculation> rebateCalculations = new();
    private int rebateCalculationId = 0;

    public IEnumerable<RebateCalculation> GetValues()
    {

        return rebateCalculations.Values;
    }

    public bool StoreCalculationResult(Rebate rebate, Product product, decimal calculatedAmount)
    {

        if (rebateCalculations.TryAdd(rebateCalculationId, new RebateCalculation
        {
            Id = rebateCalculationId,
            ProductIdentifier = product.Identifier,
            RebateIdentifier = rebate.Identifier,
            IncentiveType = rebate.Incentive,
            Amount = calculatedAmount,
            Product = product,
            Rebate = rebate
        }))
        {
            rebateCalculationId++;
            return true;
        }
        else
        {
            return false;
        }

    }
}
