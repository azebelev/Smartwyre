using System;
using System.Linq;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Contracts;

namespace Smartwyre.DeveloperTest.Services.CommunicationService;

public class CommunicationService : ICommunicationService
{
    private readonly IRebateRepository _rebateRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRebateCalculationRepository _rebateCalculationRepository;
    private readonly IRebateService _rebateService;

    public CommunicationService(IRebateService rebateService, IRebateRepository rebateRepository,
        IProductRepository productRepository, IRebateCalculationRepository rebateCalculationRepository)
    {
        _rebateService = rebateService;
        _rebateRepository = rebateRepository;
        _productRepository = productRepository;
        _rebateCalculationRepository = rebateCalculationRepository;
    }

    public void Communicate()
    {
        while (true)
        {
            ShowRecords();

            var request = GetUserRequest();

            var result = _rebateService.Calculate(request);

            if (result.Success)
                Console.WriteLine($"The calculated amount is {result.CalculatedRebateAmount}");
            else Console.WriteLine($"The calculation is failed");

            TryShowAllSavedCalculations();


            Console.WriteLine("Do you want to calculate again? (Y/N)");

            if (Console.ReadLine().ToLower() is "n" or "no")
                break;
        }
    }

    private void ShowRecords()
    {
        Console.WriteLine("Rebates:");
        foreach (var rebate in _rebateRepository.GetAllRebates())
        {
            Console.WriteLine($"Identifier={rebate.Identifier}, Incentive={rebate.Incentive}, Amount={rebate.Amount}, Percentage={rebate.Percentage}");
        }

        Console.WriteLine("Products:");
        foreach (var product in _productRepository.GetAllProducts())
        {
            Console.WriteLine($"Id={product.Id}, Identifier={product.Identifier}, Price={product.Price}, Uom={product.Uom}, SupportedIncentives={product.SupportedIncentives}");
        }
    }

    private CalculateRebateRequest GetUserRequest()
    {
        Console.WriteLine("Enter Rebate Identifier:");
        var rebateIdentifier = Console.ReadLine();

        Console.WriteLine("Enter Product Identifier:");
        var productIdentifier = Console.ReadLine();

        var request = new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
        };

        if (_rebateService.IsVolumeRequiredForCalculation(request))
        {
            while (true)
            {
                Console.WriteLine("Enter Volume:");

                if (decimal.TryParse(Console.ReadLine(), out decimal volume))
                {
                    request.Volume = volume;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid volume input. Please enter a valid decimal number.");
                }
            }
        }

        return request;
    }

    private void TryShowAllSavedCalculations()
    {
        var rebateCalculations = _rebateCalculationRepository.GetValues().ToList();

        if (rebateCalculations.Count != 0)
        {
            Console.WriteLine("All rebate calculations");
            foreach (var calculation in rebateCalculations)
            {
                Console.WriteLine(calculation);
            }
        }
    }
}