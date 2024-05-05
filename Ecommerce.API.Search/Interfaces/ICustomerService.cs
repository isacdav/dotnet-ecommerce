using Ecommerce.API.Search.Models;

namespace Ecommerce.API.Search.Interfaces;

public interface ICustomerService
{
    Task<(bool IsSuccess, Customer? Customer, string ErrorMessage)> GetCustomerAsync(int customerId);
}
