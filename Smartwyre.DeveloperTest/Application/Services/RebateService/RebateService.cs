using System;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Application.Factories;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Contracts;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTestDeveloperTest.Application.Services.RebateService;

public class RebateService : IRebateService
{
    private readonly IRebateRepository _rebateRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRebateCalculationRepository _rebateCalculationRepository;

    public RebateService(IRebateRepository rebateRepository,
        IProductRepository productRepository, IRebateCalculationRepository rebateCalculationRepository)
    {
        _rebateRepository = rebateRepository;
        _productRepository = productRepository;
        _rebateCalculationRepository = rebateCalculationRepository;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();

        var rebate = _rebateRepository.GetRebate(request.RebateIdentifier);

        if (rebate == null)
        {
            Console.WriteLine("Validation error: The rebate is not found");
            return result;
        }

        var product = _productRepository.GetProduct(request.ProductIdentifier);

        var calculator = CalculationScenarioFactory.Create(rebate.Incentive);

        result = calculator.Calculate(rebate, product, request);

        if (result.Success)
        {
            _rebateCalculationRepository.StoreCalculationResult(rebate, product, result.CalculatedRebateAmount);
        }

        return result;
    }

    public bool IsVolumeRequiredForCalculation(CalculateRebateRequest request)
    {
        var rebate = _rebateRepository.GetRebate(request.RebateIdentifier);

        if (rebate == null)
        {
            Console.WriteLine("Validation error: The rebate is null");
            return false;
        }

        return rebate.Incentive != IncentiveType.FixedCashAmount;
    }
}
