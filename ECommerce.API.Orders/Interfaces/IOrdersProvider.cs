using ECommerce.API.Orders.Models;

namespace ECommerce.API.Orders.Interfaces;

public interface IOrdersProvider
{
    Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId);
}
