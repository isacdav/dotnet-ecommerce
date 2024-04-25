using Ecommerce.API.Search.Interfaces;

namespace Ecommerce.API.Search.Services;

public class SearchService : ISearchService
{
    private readonly IOrdersService _ordersService;
    private readonly IProductsService _productsService;

    public SearchService(IOrdersService ordersService, IProductsService productsService)
    {
        _ordersService = ordersService;
        _productsService = productsService;
    }

    public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
    {
        var ordersResult = await _ordersService.GetOrderAsync(customerId);
        var productsResult = await _productsService.GetProductsAsync();

        if (ordersResult.IsSuccess)
        {
            var productDictionary = productsResult.IsSuccess
                ? productsResult.Products.ToDictionary(p => p.Id, p => p.Name)
                : new Dictionary<int, string>();

            foreach (var order in ordersResult.Orders)
            {
                foreach (var item in order.Items)
                {
                    item.ProductName = productDictionary.TryGetValue(item.ProductId, out var productName)
                        ? productName
                        : "Product information is not available";
                }
            }

            var result = new
            {
                Orders = ordersResult.Orders
            };
            return (true, result);
        }
        return (false, null);
    }
}
