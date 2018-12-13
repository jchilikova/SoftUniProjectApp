using System.Globalization;
using System.Linq;
using AutoMapper;
using Foodnetic.Models;
using Foodnetic.ViewModels.Grocery;
using Foodnetic.ViewModels.Menu;
using Foodnetic.ViewModels.Products;
using Foodnetic.ViewModels.Recipes;

namespace Foodnetic.App.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, CreateProductViewModel>().ReverseMap();

            CreateMap<Grocery, CreateGroceryViewModel>()
                .ForPath(e => e.Products, opt => opt.Ignore())
                .ForPath(e => e.ProductName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<AllRecipesViewModel, Recipe>().ReverseMap();

            CreateMap<RecipeViewModel, Recipe>().ReverseMap();

            CreateMap<Grocery, GroceryViewModel>()
                //.ForPath(e => e.Name, opt => opt.MapFrom(src => src.Name))
                //.ForPath(e => e.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate)
                //.ForPath(e => e.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();
        }
    }
}