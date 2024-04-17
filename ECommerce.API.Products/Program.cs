using ECommerce.API.Products.Db;
using ECommerce.API.Products.Interfaces;
using ECommerce.API.Products.Providers;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ProductsDbContext>(
    options =>
    {
        options.UseInMemoryDatabase("products");
    });

builder.Services.AddScoped<IProductsProvider, ProductsProvider>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

app.Run();
