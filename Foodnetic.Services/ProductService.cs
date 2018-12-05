using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly FoodneticDbContext dbContext;

        public ProductService(IMapper mapper, FoodneticDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public void Create(CreateProductViewModel bindingModel)
        {
            var product = this.mapper.Map<Product>(bindingModel);

            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();
        }

        public ICollection<Product> GetAll()
        {
            return this.dbContext.Products.ToList();
        }
    }
}
