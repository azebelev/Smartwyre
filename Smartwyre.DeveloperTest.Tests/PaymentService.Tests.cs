
using AutoFixture;
using Moq;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Application.Services.RebateService.Models;
using Smartwyre.DeveloperTest.Contracts;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTestDeveloperTest.Application.Services.RebateService;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private static readonly Fixture _fixture = new ();
    public Mock<IRebateRepository> rebateRepoMock;
    public Mock<IProductRepository> productRepoMock;
    public Mock<IRebateCalculationRepository> rebateCalcRepoMock;
    public IRebateService rebateCalcService;

    public PaymentServiceTests()
    {
        rebateRepoMock = new Mock<IRebateRepository>();
        productRepoMock = new Mock<IProductRepository>();
        rebateCalcRepoMock = new Mock<IRebateCalculationRepository>();

        rebateCalcRepoMock.Setup(r => r.StoreCalculationResult(null, null, _fixture.Create<decimal>())).Returns(true);

        rebateCalcService = new RebateService(
            rebateRepoMock.Object,
            productRepoMock.Object,
            rebateCalcRepoMock.Object);
    }

    public record CalculationScenarioTestCase(
        string TestCaseName,
        decimal RequestVolume,
        Rebate Rebate,
        Product Product,
        CalculateRebateResult ExpectedResult);

    protected void MockRepositoryValues(Rebate rebate, Product product)
    {
        rebateRepoMock.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(rebate);
        productRepoMock.Setup(r => r.GetProduct(It.IsAny<string>())).Returns(product);
    }

    [Theory]
    [MemberData(nameof(CommonCalculationTestCases))]
    public void CommonCalculation_Test(CalculationScenarioTestCase testCase)
    {
        MockRepositoryValues(testCase.Rebate, testCase.Product);

        var result = rebateCalcService.Calculate(
            new CalculateRebateRequest { Volume = testCase.RequestVolume });

        Assert.Equal(result.Success, testCase.ExpectedResult.Success);

        if (result.Success)
            Assert.Equal(result.CalculatedRebateAmount, testCase.ExpectedResult.CalculatedRebateAmount);
    }

    public static TheoryData<CalculationScenarioTestCase> CommonCalculationTestCases() => new()
        {
            new CalculationScenarioTestCase(
                "The Rebate = null",
                _fixture.Create<decimal>(),
                null,
                new Product(),
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "The Product = null",
                _fixture.Create<decimal>(),
                new Rebate(),
                null,
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Not supported incentiveType",
                _fixture.Create<decimal>(),
                new Rebate { Incentive = IncentiveType.FixedRateRebate },
                new Product { SupportedIncentives = SupportedIncentiveType.AmountPerUom | SupportedIncentiveType.FixedCashAmount},
                new CalculateRebateResult { Success = false }),
        };

    [Theory]
    [MemberData(nameof(AmountPerUomCalculationTestCases))]
    public void AmountPerUomCalculation_Test(CalculationScenarioTestCase testCase)
    {
        MockRepositoryValues(testCase.Rebate, testCase.Product);

        var result = rebateCalcService.Calculate(
            new CalculateRebateRequest { Volume = testCase.RequestVolume });

        Assert.Equal(result.Success, testCase.ExpectedResult.Success);

        if (result.Success)
            Assert.Equal(result.CalculatedRebateAmount, testCase.ExpectedResult.CalculatedRebateAmount);
    }

    public static TheoryData<CalculationScenarioTestCase> AmountPerUomCalculationTestCases() => new()
        {
            new CalculationScenarioTestCase(
                "Product price = 0",
                _fixture.Create<decimal>(),
                new Rebate { Incentive = IncentiveType.AmountPerUom },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.AmountPerUom,
                    Price = 0
                },
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Request volume = 0",
                0,
                new Rebate { Incentive = IncentiveType.AmountPerUom },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.AmountPerUom,
                    Price = _fixture.Create<decimal>()
                },
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Valid parameters",
                100,
                new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 10 },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.AmountPerUom | SupportedIncentiveType.FixedCashAmount,
                    Price = 10
                },
                new CalculateRebateResult
                {
                    Success = true,
                    CalculatedRebateAmount = 1000
                }),
        };

    [Theory]
    [MemberData(nameof(FixedCashAmountCalculationTestCases))]
    public void FixedCashAmountCalculation_Test(CalculationScenarioTestCase testCase)
    {
        MockRepositoryValues(testCase.Rebate, testCase.Product);

        var result = rebateCalcService.Calculate(
            new CalculateRebateRequest { Volume = testCase.RequestVolume });

        Assert.Equal(result.Success, testCase.ExpectedResult.Success);

        if (result.Success)
            Assert.Equal(result.CalculatedRebateAmount, testCase.ExpectedResult.CalculatedRebateAmount);
    }

    public static TheoryData<CalculationScenarioTestCase> FixedCashAmountCalculationTestCases() => new()
        {
            new CalculationScenarioTestCase(
                "Rebate amount = 0",
                _fixture.Create<decimal>(),
                new Rebate
                {
                    Amount = 0,
                    Incentive = IncentiveType.FixedCashAmount
                },
                new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount},
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Valid parameters",
                _fixture.Create<decimal>(),
                new Rebate
                {
                    Amount = 134,
                    Incentive = IncentiveType.FixedCashAmount
                },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom
                },
                new CalculateRebateResult
                {
                    Success = true,
                    CalculatedRebateAmount = 134
                })
        };

    [Theory]
    [MemberData(nameof(FixedRateRebateCalculationTestCases))]
    public void FixedRateRebateCalculation_Test(CalculationScenarioTestCase testCase)
    {
        MockRepositoryValues(testCase.Rebate, testCase.Product);

        var result = rebateCalcService.Calculate(
            new CalculateRebateRequest { Volume = testCase.RequestVolume });

        Assert.Equal(result.Success, testCase.ExpectedResult.Success);

        if (result.Success)
            Assert.Equal(result.CalculatedRebateAmount, testCase.ExpectedResult.CalculatedRebateAmount);
    }

    public static TheoryData<CalculationScenarioTestCase> FixedRateRebateCalculationTestCases() => new()
        {
            new CalculationScenarioTestCase(
                "Percentage = 0",
                _fixture.Create<decimal>(),
                new Rebate
                {
                    Incentive = IncentiveType.FixedRateRebate,
                    Percentage = 0
                },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                    Price = _fixture.Create<decimal>()
                },
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Price = 0",
                _fixture.Create<decimal>(),
                new Rebate
                {
                    Incentive = IncentiveType.FixedRateRebate,
                    Percentage = _fixture.Create<decimal>()
                },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                    Price = 0
                },
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Volume = 0",
                0,
                new Rebate
                {
                    Incentive = IncentiveType.FixedRateRebate,
                    Percentage = _fixture.Create<decimal>()
                },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                    Price = _fixture.Create<decimal>()
                },
                new CalculateRebateResult { Success = false }),

            new CalculationScenarioTestCase(
                "Valid parameters",
                2,
                new Rebate
                {
                    Incentive = IncentiveType.FixedRateRebate,
                    Percentage = 0.1m
                },
                new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                    Price = 10
                },
                new CalculateRebateResult
                {
                    Success = true,
                    CalculatedRebateAmount = 2
                }),
        };
}
