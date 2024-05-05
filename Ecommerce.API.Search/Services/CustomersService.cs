using System.Text.Json;

using Ecommerce.API.Search.Interfaces;
using Ecommerce.API.Search.Models;

namespace Ecommerce.API.Search.Services;

public class CustomersService : ICustomerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrdersService> _logger;

    public CustomersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, Customer? Customer, string ErrorMessage)> GetCustomerAsync(int customerId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CustomersService");
            var response = await client.GetAsync($"api/customers/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<Customer>(content, options);
                return (true, result, string.Empty);
            }
            return (false, null, response.ReasonPhrase ?? "Failed to get customer");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return (false, null, ex.Message);
        }
    }

}
