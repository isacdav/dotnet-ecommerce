using ECommerce.API.Products.Models;

namespace ECommerce.API.Products.Interfaces;

public interface IProductsProvider
{
    Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
    Task<(bool IsSuccess, Product? Product, string ErrorMessage)> GetProductAsync(int id);
}
