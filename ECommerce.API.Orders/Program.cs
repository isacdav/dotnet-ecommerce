using ECommerce.API.Orders.Db;
using ECommerce.API.Orders.Interfaces;
using ECommerce.API.Orders.Providers;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<OrdersDbContext>(
    options =>
    {
        options.UseInMemoryDatabase("orders");
    });

builder.Services.AddScoped<IOrdersProvider, OrdersProvider>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

app.Run();
