using AutoMapper;
using Foodnetic.Models;
using Foodnetic.ViewModels.Grocery;
using Foodnetic.ViewModels.Products;

namespace Foodnetic.App.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, CreateProductViewModel>().ReverseMap();

            CreateMap<Grocery, CreateGroceryViewModel>()
                .ForPath(e => e.Products, opt => opt.Ignore())
                .ForPath(e => e.Product, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}