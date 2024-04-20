using ECommerce.API.Customers.Models;

namespace ECommerce.API.Customers.Interfaces;

public interface ICustomersProvider
{
    Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync();
    Task<(bool IsSuccess, Customer? Customer, string ErrorMessage)> GetCustomerAsync(int id);
}
