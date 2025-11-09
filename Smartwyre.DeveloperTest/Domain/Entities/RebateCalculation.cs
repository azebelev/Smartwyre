using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Domain.Entities;

public class RebateCalculation
{
    public int Id { get; set; }
    public string ProductIdentifier { get; set; }
    public string RebateIdentifier { get; set; }
    public IncentiveType IncentiveType { get; set; }
    public decimal Amount { get; set; }

    public Product Product { get; set; }
    public Rebate Rebate { get; set; }


    public override string ToString() =>
        $"RebateCalculation(Id={Id}, Product='{ProductIdentifier}', Rebate='{RebateIdentifier}', IncentiveType={IncentiveType}, Amount={Amount})";
}
