using System.Linq;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Tests.ProductServiceTests
{
    public class ProductServiceTests : BaseService
    {
        private IProductService ProductService => this.ServiceProvider.GetRequiredService<IProductService>();

        [Test]
        public void CreateProductShouldAddProductToDatabase()
        {
            var product = new Product
            {
                Name = "strawberry",
                ProductType = ProductType.Fruits
            };

            this.ProductService.Create(product);

            var count = this.DbContext.Products.Count();

            Assert.AreEqual(1, count);
        }

        [Test]
        [TestCase("strawberry")]
        [TestCase("milk")]
        [TestCase("eggs")]
        public void CheckIfProductExistsByNameShouldReturnTrueIfProductExists(string name)
        {
            var product = new Product
            {
                Name = name,
                ProductType = ProductType.Fruits
            };

            this.ProductService.Create(product);

            var exists = this.ProductService.CheckIfProductExists(name);

            Assert.IsTrue(exists);
        }

        [Test]
        public void CheckIfProductExistsByNameShouldReturnFalseIfProductDoesNotExists()
        {
            var product = new Product
            {
                Name = "tomato",
                ProductType = ProductType.Fruits
            };

            this.ProductService.Create(product);

            var exists = this.ProductService.CheckIfProductExists("milk");

            Assert.IsFalse(exists);
        }

        [Test]
        public void GetAllProductsShouldReturnAllProductsFromDatabase()
        {
            for (int i = 0; i < 10; i++)
            {
                var product = new Product
                {
                    Name = i.ToString(),
                    ProductType = ProductType.Fruits
                };

                this.DbContext.Products.Add(product);
                this.DbContext.SaveChanges();
            }

            var count = this.ProductService.GetAll().Count;

            Assert.AreEqual(10, count);
        }

        [Test]
        public void GetAllProductsShouldReturnNullIfThereAreNotProducts()
        {
            var result = this.ProductService.GetAll();

            Assert.AreEqual(null, result);
        }
    }
}
