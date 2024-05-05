﻿using System.Text.Json;

using Ecommerce.API.Search.Interfaces;
using Ecommerce.API.Search.Models;

namespace Ecommerce.API.Search;

public class ProductsService : IProductsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OrdersService> _logger;

    public ProductsService(IHttpClientFactory httpClientFactory, ILogger<OrdersService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<(bool IsSuccess, IEnumerable<Product>? Products, string ErrorMessage)> GetProductsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ProductsService");
            var response = await client.GetAsync("api/products");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IEnumerable<Product>>(content, options);
                return (true, result, string.Empty);
            }
            return (false, null, response.ReasonPhrase ?? "Failed to get products");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return (false, null, ex.Message);
        }
    }
}
