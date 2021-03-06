﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Groceries;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly FoodneticDbContext dbContext;

        public FridgeService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateGrocery(CreateGroceryViewModel bindingModel, string username)
        {
            var product = this.dbContext.Products.FirstOrDefault(x => x.Name == bindingModel.ProductName);

            var user = (FoodneticUser)this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            var fridge = this.dbContext.VirtualFridges.Include(f => f.Groceries).FirstOrDefault(x => x.OwnerId == user.Id);

            if (fridge == null)
            {
                fridge = new VirtualFridge
                {
                    OwnerId = user.Id,
                };

                this.dbContext.VirtualFridges.Add(fridge);

            }

            Grocery grocery;

            if (this.CheckIfGroceryExists(bindingModel.ProductName, fridge))
            {
                grocery = fridge.Groceries.FirstOrDefault(g => g.Name == bindingModel.ProductName);

                grocery.Quantity += bindingModel.Quantity;
                grocery.ExpirationDate = bindingModel.ExpirationDate;
            }
            else
            {
                grocery = new Grocery
                {
                    Name = product.Name,
                    ProductType = product.ProductType,
                    ExpirationDate = bindingModel.ExpirationDate,
                    Quantity = bindingModel.Quantity
                };

                fridge.Groceries.Add(grocery);
            }
           
            this.dbContext.SaveChanges();
        }

        private bool CheckIfGroceryExists(string productName,VirtualFridge fridge)
        {
            return fridge.Groceries.Any(g => g.Name == productName);
        }

        public IEnumerable<Grocery> GetAllGroceries(string username)
        {
            var user = (FoodneticUser)this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            var virtualFridgeId = this.dbContext.VirtualFridges.FirstOrDefault(f => f.OwnerId == user.Id)?.Id;

            if (virtualFridgeId == null)
            {
                return null;
            }

            var groceries = this.dbContext.Groceries.Where(x => x.VirtualFridgeId == virtualFridgeId);

            return groceries;
        }
    }
}
