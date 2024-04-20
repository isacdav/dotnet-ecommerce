using AutoMapper;

using ECommerce.API.Orders.Db;
using ECommerce.API.Orders.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Orders.Providers;

public class OrdersProvider : IOrdersProvider
{
    private readonly OrdersDbContext dbContext;
    private readonly ILogger<OrdersProvider> logger;
    private readonly IMapper mapper;

    public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;

        SeedData();
    }

    private void SeedData()
    {
        if (!dbContext.Orders.Any())
        {
            dbContext.Orders.Add(new Db.Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<Db.OrderItem>
                {
                    new Db.OrderItem { Id = 1, ProductId = 1, Quantity = 1, UnitPrice = 10 },
                    new Db.OrderItem { Id = 2, ProductId = 2, Quantity = 2, UnitPrice = 20 }
                }
            });

            dbContext.Orders.Add(new Db.Order
            {
                Id = 2,
                CustomerId = 2,
                OrderDate = DateTime.Now,
                Items = new List<Db.OrderItem>
                {
                    new Db.OrderItem { Id = 3, ProductId = 3, Quantity = 3, UnitPrice = 30 },
                    new Db.OrderItem { Id = 4, ProductId = 4, Quantity = 4, UnitPrice = 40 }
                }
            });

            dbContext.SaveChanges();
        }
    }

    public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
    {
        try
        {
            var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).Include(o => o.Items).ToListAsync();
            if (orders != null && orders.Any())
            {
                var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                return (true, result, string.Empty);
            }

            return (false, Enumerable.Empty<Models.Order>(), "Not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return (false, Enumerable.Empty<Models.Order>(), ex.Message);
        }
    }

}
