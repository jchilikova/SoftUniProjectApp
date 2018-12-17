using System.Collections.Generic;
using Foodnetic.Models;

namespace Foodnetic.Services.Contracts
{
    public interface IProductService
    {
        void Create(Product bindingModel);

        ICollection<Product> GetAll();

        bool CheckIfProductExists(string name);
    }
}
