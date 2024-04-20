using ECommerce.API.Customers.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Customers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomersProvider _customersProvider;

    public CustomersController(ICustomersProvider customersProvider)
    {
        _customersProvider = customersProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        var result = await _customersProvider.GetCustomersAsync();
        return result.IsSuccess ? Ok(result.Customers) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerAsync(int id)
    {
        var result = await _customersProvider.GetCustomerAsync(id);
        return result.IsSuccess ? Ok(result.Customer) : NotFound();
    }

}

