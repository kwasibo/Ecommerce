using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Api.Products.Models;

namespace Ecommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSucess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();

        Task<(bool IsSucess, Product Product, string ErrorMessage)> GetProductAsync(int id);
    }
}
