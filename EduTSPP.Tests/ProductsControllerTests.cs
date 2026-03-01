using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductsControllerTests
{
    private DbContextOptions<AppDb> GetOptions()
    {
        // Кожен виклик створює нову чисту базу в пам'яті
        return new DbContextOptionsBuilder<AppDb>()
            .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetProducts_ReturnsEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var options = GetOptions();
        using var context = new AppDb(options);
        var controller = new ProductsController(context);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(actionResult.Value);
        Assert.Empty(products); // Перевіряємо, що список порожній
    }

    [Fact]
    public async Task GetProducts_ReturnsCorrectData()
    {
        // Arrange
        var options = GetOptions();
        var testProduct = new Product { Id = 1, Name = "Ноутбук", Price = 25000, Category = "Tech", Vendor = "Asus" };
        
        using (var context = new AppDb(options))
        {
            context.Products.Add(testProduct);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new AppDb(options))
        {
            var controller = new ProductsController(context);
            var result = await controller.GetProducts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            var products = actionResult.Value!; // '!' прибирає попередження CS8604
            
            var product = Assert.Single(products);
            Assert.Equal("Ноутбук", product.Name);
            Assert.Equal(25000, product.Price);
        }
    }
}