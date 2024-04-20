using AutoMapper;

using ECommerce.API.Customers.Db;
using ECommerce.API.Customers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Customers.Providers;

public class CustomersProvider : ICustomersProvider
{
    private readonly CustomersDbContext _dbContext;
    private readonly ILogger<CustomersProvider> _logger;
    private readonly IMapper _mapper;

    public CustomersProvider(CustomersDbContext context, ILogger<CustomersProvider> logger, IMapper mapper)
    {
        _dbContext = context;
        _logger = logger;
        _mapper = mapper;

        SeedData();
    }

    private void SeedData()
    {
        if (!_dbContext.Customers.Any())
        {
            _dbContext.Customers.AddRange(
                new Customer { Id = 1, Name = "John Doe", Address = "123 Main St", },
                new Customer { Id = 2, Name = "Jane Oed", Address = "456 Elm St", }
            );
            _dbContext.SaveChanges();
        }
    }

    public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
    {
        try
        {
            var customers = await _dbContext.Customers.ToListAsync();
            if (customers != null && customers.Any())
            {
                var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<Models.Customer>>(customers);
                return (true, result, string.Empty);
            }
            return (false, Enumerable.Empty<Models.Customer>(), "Not Found");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            return (false, Enumerable.Empty<Models.Customer>(), ex.Message);
        }
    }

    public async Task<(bool IsSuccess, Models.Customer? Customer, string ErrorMessage)> GetCustomerAsync(int id)
    {
        try
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
            if (customer != null)
            {
                var result = _mapper.Map<Customer, Models.Customer>(customer);
                return (true, result, string.Empty);
            }
            return (false, null, "Not Found");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            return (false, null, ex.Message);
        }
    }

}
