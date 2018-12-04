using Foodnetic.Models.Enums;

namespace Foodnetic.App.Areas.Administration.Models.Products
{
    public class CreateProductViewModel
    {
        public string Name { get; set; }

        public ProductType ProductType { get; set; }
    }
}
