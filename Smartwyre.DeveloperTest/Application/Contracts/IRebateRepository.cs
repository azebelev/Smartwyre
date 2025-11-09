using System.Collections.Generic;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Application.Contracts;

public interface IRebateRepository
{
    Rebate GetRebate(string rebateIdentifier);
    void StoreCalculationResult(Rebate account, decimal rebateAmount);
    IReadOnlyCollection<Rebate> GetAllRebates();
}