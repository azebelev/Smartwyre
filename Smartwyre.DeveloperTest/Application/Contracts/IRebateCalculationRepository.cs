using System.Collections.Generic;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Application.Contracts;

public interface IRebateCalculationRepository
{
    IEnumerable<RebateCalculation> GetValues();

    bool StoreCalculationResult(Rebate rebate, Product product, decimal calculatedAmount);
}