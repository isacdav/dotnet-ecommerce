using Ecommerce.API.Search;
using Ecommerce.API.Search.Interfaces;
using Ecommerce.API.Search.Services;

using Polly;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddHttpClient("OrdersService", config =>
{
    config.BaseAddress = new Uri(configuration["Services:Orders"]);
});

builder.Services.AddHttpClient("ProductsService", config =>
{
    config.BaseAddress = new Uri(configuration["Services:Products"]);
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)));

var app = builder.Build();

app.MapControllers();

app.Run();
