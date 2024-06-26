﻿using AutoMapper;

using ECommerce.API.Products.Db;
using ECommerce.API.Products.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Products.Providers;

public class ProductsProvider : IProductsProvider
{
    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<ProductsProvider> _logger;
    private readonly IMapper _mapper;

    public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;

        SeedData();
    }

    private void SeedData()
    {
        if (!_dbContext.Products.Any())
        {
            _dbContext.Add(new Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
            _dbContext.Add(new Product() { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
            _dbContext.Add(new Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 100 });
            _dbContext.Add(new Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 100 });
            _dbContext.SaveChanges();
        }
    }

    public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
    {
        try
        {
            var products = await _dbContext.Products.ToListAsync();
            if (products != null && products.Any())
            {
                var result = _mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                return (true, result, string.Empty);
            }
            return (false, Enumerable.Empty<Models.Product>(), "Not Found");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            return (false, Enumerable.Empty<Models.Product>(), ex.Message);
        }
    }

    public async Task<(bool IsSuccess, Models.Product? Product, string ErrorMessage)> GetProductAsync(int id)
    {
        try
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                var result = _mapper.Map<Db.Product, Models.Product>(product);
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
