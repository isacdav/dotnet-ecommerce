using ECommerce.API.Products.Db;
using ECommerce.API.Products.Providers;
using ECommerce.API.Products.Profiles;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ECommerce.API.Products.Tests;

public class ProductsServiceTests
{
    private ProductsDbContext _dbContext;
    private ProductsProvider _productsProvider;

    public ProductsServiceTests()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ProductsDbContext(options);

        CreateProducts(_dbContext);

        var productsProfile = new ProductProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));

        var mapper = new Mapper(configuration);

        _productsProvider = new ProductsProvider(_dbContext, null, mapper);
    }

    private void CreateProducts(ProductsDbContext dbContext)
    {
        for (int i = 0; i <= 10; i++)
        {
            dbContext.Products.Add(new Product
            {
                Name = Guid.NewGuid().ToString(),
                Inventory = i + 10,
                Price = (decimal)(i * 3.14)
            });
        }

        dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetProductsReturnAllProducts()
    {
        var products = await _productsProvider.GetProductsAsync();

        Assert.True(products.IsSuccess);
        Assert.True(products.Products.Any());
        Assert.Empty(products.ErrorMessage);
    }

    [Fact]
    public async Task GetProductsReturnProductUsingValidId()
    {
        var product = await _productsProvider.GetProductAsync(1);

        Assert.True(product.IsSuccess);
        Assert.NotNull(product.Product);
        Assert.True(product.Product.Id == 1);
        Assert.Empty(product.ErrorMessage);
    }

    [Fact]
    public async Task GetProductsReturnProductUsingInvalidId()
    {
        var product = await _productsProvider.GetProductAsync(-1);

        Assert.False(product.IsSuccess);
        Assert.Null(product.Product);
        Assert.NotNull(product.ErrorMessage);
    }
}
