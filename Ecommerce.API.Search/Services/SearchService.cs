using Ecommerce.API.Search.Interfaces;

namespace Ecommerce.API.Search.Services;

public class SearchService : ISearchService
{
    private readonly IOrdersService _ordersService;
    private readonly IProductsService _productsService;
    private readonly ICustomerService _customerService;

    public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomerService customerService)
    {
        _ordersService = ordersService;
        _productsService = productsService;
        _customerService = customerService;
    }

    public async Task<(bool IsSuccess, dynamic? SearchResults)> SearchAsync(int customerId)
    {
        var ordersResult = await _ordersService.GetOrderAsync(customerId);
        var productsResult = await _productsService.GetProductsAsync();
        var customerResult = await _customerService.GetCustomerAsync(customerId);

        if (ordersResult.IsSuccess)
        {
            var productDictionary = productsResult.IsSuccess && productsResult.Products != null
                ? productsResult.Products.ToDictionary(p => p.Id, p => p.Name)
                : new Dictionary<int, string>();

            if (ordersResult.Orders != null)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productDictionary.TryGetValue(item.ProductId, out var productName)
                            ? productName
                            : "Product information is not available";
                    }
                }
            }

            var result = new
            {
                Customer = customerResult.IsSuccess ? customerResult.Customer : new { Name = "Customer information is not available" } as object,
                Orders = ordersResult.Orders
            };
            return (true, result);
        }

        return (false, null);
    }
}
