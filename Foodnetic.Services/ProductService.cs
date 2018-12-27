using System.Collections.Generic;
using System.Linq;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;

namespace Foodnetic.Services
{
    public class ProductService : IProductService
    {
        private readonly FoodneticDbContext dbContext;

        public ProductService(FoodneticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(Product product)
        {
            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();
        }

        public bool CheckIfProductExists(string name)
        {
            return this.dbContext.Products.Any(x => x.Name == name && x.GetType() == typeof(Product));
        }

        public ICollection<Product> GetAll()
        {
            if (this.dbContext.Products.Any())
            {
                return this.dbContext.Products.Where(x => x.GetType() == typeof(Product)).ToList();
            }

            return null;
        }
           
    }
}
