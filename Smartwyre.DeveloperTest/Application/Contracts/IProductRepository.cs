using System.Collections.Generic;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Application.Contracts;

public interface IProductRepository
{
    Product GetProduct(string productIdentifier);

    IReadOnlyCollection<Product> GetAllProducts();
}