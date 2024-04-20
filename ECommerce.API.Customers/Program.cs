using ECommerce.API.Customers.Db;
using ECommerce.API.Customers.Interfaces;
using ECommerce.API.Customers.Providers;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<CustomersDbContext>(
    options =>
    {
        options.UseInMemoryDatabase("customers");
    });

builder.Services.AddScoped<ICustomersProvider, CustomersProvider>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

app.Run();
