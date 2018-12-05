using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Grocery;

namespace Foodnetic.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IMapper mapper;
        private readonly FoodneticDbContext dbContext;

        public FridgeService(IMapper mapper, FoodneticDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public void CreateGrocery(CreateGroceryViewModel bindingmodel, string username)
        {
            var product = this.dbContext.Products.FirstOrDefault(x => x.Name == bindingmodel.Product);

            User user = (User)this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            var grocery = new Grocery
            {
                Name = product.Name,
                ProductType = product.ProductType,
                ExpirationDate = bindingmodel.ExpirationDate,
                Quantity = bindingmodel.Quantity
            };

            var fridge = this.dbContext.VirtualFridges.FirstOrDefault(x => x.OwnerId == user.Id);

            if (fridge == null)
            {
                fridge = new VirtualFridge
                {
                    OwnerId = user.Id,
                };

                this.dbContext.VirtualFridges.Add(fridge);

            }

            fridge.Groceries.Add(grocery);
            this.dbContext.SaveChanges();


        }

        public IEnumerable<Grocery> GetAll(string name)
        {
            User user = (User)this.dbContext.Users.FirstOrDefault(x => x.UserName == name);
            var virtualFridgeId = this.dbContext.VirtualFridges.FirstOrDefault(f => f.OwnerId == user.Id).Id;

            var groceries = this.dbContext.Groceries.Where(x => x.VirtualFridgeId == virtualFridgeId);

            return groceries;
        }
    }
}
