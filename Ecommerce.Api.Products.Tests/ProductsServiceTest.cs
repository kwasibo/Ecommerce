using AutoMapper;
using Ecommerce.Api.Products.Db;
using Ecommerce.Api.Products.Profiles;
using Ecommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        public ProductsDbContext dbContext;

        public ProductsServiceTest()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
               .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
               .Options;
            dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
        }

        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {           

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            
            var product = await productsProvider.GetProductsAsync();
            Assert.True(product.IsSucess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {
           
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(1);
            Assert.True(product.IsSucess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInValidId()
        {
           
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductAsync(-1);
            Assert.False(product.IsSucess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            var product = dbContext.Products.FirstOrDefault();

        if (product == null)
           {
                for (int i = 1; i <= 10; i++)
                {
                    dbContext.Products.Add(new Product()
                    {
                        Id = i,
                        Name = Guid.NewGuid().ToString(),
                        Inventory = i + 10,
                        Price = (decimal)(i * 3.14)
                    });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
