using System.Collections.Generic;
using Smartwyre.DeveloperTest.Application.Contracts;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Persistence;

public class ProductRepository: IProductRepository
{
    private readonly Dictionary<string, Product> products = new Dictionary<string, Product>();

    private readonly SupportedIncentiveType AllSupportedIncentives = SupportedIncentiveType.FixedRateRebate |
        SupportedIncentiveType.AmountPerUom |
        SupportedIncentiveType.FixedCashAmount;

    public ProductRepository()
    {
        products.Add("product1", new Product { Id = 1, Identifier = "product1", Price = 100.00m, Uom = "Each", SupportedIncentives = AllSupportedIncentives });
        products.Add("product2", new Product { Id = 2, Identifier = "product2", Price = 150.00m, Uom = "Some", SupportedIncentives = AllSupportedIncentives });
        products.Add("product3", new Product { Id = 2, Identifier = "product3", Price = 140.00m, Uom = "Some", SupportedIncentives = AllSupportedIncentives });
    }
    public Product GetProduct(string productIdentifier)
    {
        products.TryGetValue(productIdentifier, out var product);

        return product;
    }

    public IReadOnlyCollection<Product> GetAllProducts()
    {
        return products.Values;
    }
}
