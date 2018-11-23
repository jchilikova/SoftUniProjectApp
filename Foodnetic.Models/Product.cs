using System;
using Foodnetic.Models.Enums;

namespace Foodnetic.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ProductType ProductType { get; set; }
    }
}
