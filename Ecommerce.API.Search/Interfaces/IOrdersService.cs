using Ecommerce.API.Search.Models;

namespace Ecommerce.API.Search.Interfaces;

public interface IOrdersService
{
    Task<(bool IsSuccess, IEnumerable<Order>? Orders, string ErrorMessage)> GetOrderAsync(int customerId);
}
