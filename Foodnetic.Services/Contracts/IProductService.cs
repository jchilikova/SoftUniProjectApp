using System;
using System.Collections.Generic;
using System.Text;
using Foodnetic.Models;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.Services.Contracts
{
    public interface IProductService
    {
        void Create(CreateProductViewModel bindingModel);
        ICollection<Product> GetAll();
    }
}
