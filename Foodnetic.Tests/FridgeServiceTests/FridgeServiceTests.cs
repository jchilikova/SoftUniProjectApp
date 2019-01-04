using System;
using System.Linq;
using Foodnetic.Models;
using Foodnetic.Models.Enums;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Groceries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Services.Tests.FridgeServiceTests
{
    public class FridgeServiceTests : BaseService
    {
        private IFridgeService FridgeService => this.ServiceProvider.GetRequiredService<IFridgeService>();

        [Test]
        public void GetAllGroceriesShouldReturnAllGroceries()
        {
            var user = new FoodneticUser
            {
                Id="1",
                UserName = "test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var virtualFridge = new VirtualFridge
            {
                OwnerId = "1",
                Id = "test"
            };

            var grocery = new Grocery
            {
                ExpirationDate = DateTime.Now,
                Name = "test",
                Quantity = 40,
                VirtualFridgeId = "test"
            };

            var grocery2 = new Grocery
            {
                ExpirationDate = DateTime.Now,
                Name = "test2",
                Quantity = 40,
                VirtualFridgeId = "test"
            };


            virtualFridge.Groceries.Add(grocery);

            virtualFridge.Groceries.Add(grocery2);

            this.DbContext.VirtualFridges.Add(virtualFridge);
            this.DbContext.SaveChanges();

            var result = this.FridgeService.GetAllGroceries("test").Count();

            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetAllGroceriesShouldReturnNullIfVirtualFridgeDoesNotExists()
        {
            var user = new FoodneticUser
            {
                Id = "1",
                UserName = "test"
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            var result = this.FridgeService.GetAllGroceries("test");

            Assert.AreEqual(result, null);
        }

        [Test]
        public void CreateGroceriesShouldCreateVirtualFridgeAndAddGroceryIfVirtualFridgeDoesNotExists()
        {
            var user = new FoodneticUser
            {
                Id = "1",
                UserName = "test"
            };

            var product = new Product
            {
                Name = "test",
                ProductType = ProductType.Fruits
            };

            this.DbContext.Users.Add(user);
            this.DbContext.Products.Add(product);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateGroceryViewModel
            {
                ProductName = "test",
                ExpirationDate = DateTime.Now,
                Quantity = 100
            };

            this.FridgeService.CreateGrocery(bindingModel, "test");

            var fridge = this.DbContext.VirtualFridges.Include(f => f.Groceries).FirstOrDefault(x => x.OwnerId == "1");

            Assert.AreEqual(1, fridge.Groceries.Count);
        }

        [Test]
        public void CreateGroceryShouldAddGroceryToExistingFridge()
        {
            var user = new FoodneticUser
            {
                Id = "1",
                UserName = "test"
            };

            var product = new Product
            {
                Name = "test",
                ProductType = ProductType.Fruits
            };

            var virtualFridge = new VirtualFridge
            {
                OwnerId = "1",
                Id = "test"
            };

            var grocery = new Grocery
            {
                ExpirationDate = DateTime.Now,
                Name = "tomato",
                Quantity = 40,
                VirtualFridgeId = "test"
            };

            virtualFridge.Groceries.Add(grocery);
            this.DbContext.VirtualFridges.Add(virtualFridge);
            this.DbContext.Users.Add(user);
            this.DbContext.Products.Add(product);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateGroceryViewModel
            {
                ProductName = "test",
                ExpirationDate = DateTime.Now,
                Quantity = 100
            };

            this.FridgeService.CreateGrocery(bindingModel, "test");

            var fridge = this.DbContext.VirtualFridges.Include(f => f.Groceries).FirstOrDefault(x => x.OwnerId == "1");

            Assert.AreEqual(2, fridge.Groceries.Count);
        }

        [Test]
        public void CreateGroceryShouldSumQuantityIfGroceryAlreadyExists()
        {
            var user = new FoodneticUser
            {
                Id = "1",
                UserName = "test"
            };

            var product = new Product
            {
                Name = "test",
                ProductType = ProductType.Fruits
            };

            var virtualFridge = new VirtualFridge
            {
                OwnerId = "1",
                Id = "test"
            };

            var grocery = new Grocery
            {
                ExpirationDate = DateTime.Now,
                Name = "tomato",
                Quantity = 40,
                VirtualFridgeId = "test"
            };

            virtualFridge.Groceries.Add(grocery);
            this.DbContext.VirtualFridges.Add(virtualFridge);
            this.DbContext.Users.Add(user);
            this.DbContext.Products.Add(product);
            this.DbContext.SaveChanges();

            var bindingModel = new CreateGroceryViewModel
            {
                ProductName = "tomato",
                ExpirationDate = DateTime.Now,
                Quantity = 60
            };

            this.FridgeService.CreateGrocery(bindingModel, "test");

            var fridge = this.DbContext.VirtualFridges.Include(f => f.Groceries).FirstOrDefault(x => x.OwnerId == "1");

            Assert.AreEqual(fridge.Groceries.FirstOrDefault(x => x.Name == "tomato").Quantity, 100);
        }


    }
}
