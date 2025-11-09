using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Domain.RebateCalculationScenarios.Abstracts;

public abstract class BaseCalculationScenario
{
    protected abstract SupportedIncentiveType SupportedIncentiveType { get; }
    protected readonly ICollection<string> ValidationsErrors = new List<string>();

    public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();

        if (!ValidateBase(rebate, product))
        {
            ProcessValidationsErrors(ValidationsErrors);
            return result;
        }

        if (!Validate(rebate, product, request))
        {
            ProcessValidationsErrors(ValidationsErrors);
            return result;
        }

        result.CalculatedRebateAmount = CalculateRebateAmount(rebate, product, request);
        result.Success = true;

        return result;
    }

    protected abstract decimal CalculateRebateAmount(Rebate rebate, Product product, CalculateRebateRequest request);

    protected virtual bool Validate(Rebate rebate, Product product, CalculateRebateRequest request) => true;

    protected virtual void ProcessValidationsErrors(ICollection<string> errorMessages)
    {
        Console.WriteLine("Validations errors:");
        foreach (var errorMessage in errorMessages)
            Console.WriteLine(errorMessage);
    }

    private bool ValidateBase(Rebate rebate, Product product)
    {
        if (product == null)
        {
            ValidationsErrors.Add("The product is null");
            return false;
        }

        if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType))
        {
            ValidationsErrors.Add("The incentiveType is not supported by the product");
            return false;
        }

        return true;
    }
}
