using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Data;

public class RebateRepository : IRebateRepository
{
    private readonly Dictionary<string, Rebate> rebates = new Dictionary<string, Rebate>();

    public RebateRepository()
    {
        rebates.Add("rebate1", new Rebate { Identifier = "rebate1", Incentive = IncentiveType.FixedRateRebate, Amount = 10.00m, Percentage = 0.05m });
        rebates.Add("rebate2", new Rebate { Identifier = "rebate2", Incentive = IncentiveType.AmountPerUom, Amount = 2.00m, Percentage = 0.2m });
        rebates.Add("rebate3", new Rebate { Identifier = "rebate3", Incentive = IncentiveType.FixedCashAmount, Amount = 22.00m, Percentage = 0.1m });
    }

    public Rebate GetRebate(string rebateIdentifier)
    {
        rebates.TryGetValue(rebateIdentifier, out var rebate);

        return rebate;
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        if (rebates.TryGetValue(account.Identifier, out Rebate value))
        {
            value.Amount = rebateAmount;
            Console.WriteLine($"Rebate amount of {account.Identifier} updated, new Amount = {value.Amount}");
        }
    }

    public IReadOnlyCollection<Rebate> GetAllRebates()
    {
        return rebates.Values;
    }
}
