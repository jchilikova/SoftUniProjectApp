using AutoMapper;
using Foodnetic.Models;
using Foodnetic.ViewModels.Account;
using Foodnetic.ViewModels.Contact;
using Foodnetic.ViewModels.Groceries;
using Foodnetic.ViewModels.Grocery;
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

            CreateMap<RecipeViewModel, Recipe>()
                .ForPath(e => e.Ingredients, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Grocery, GroceryViewModel>().ReverseMap();

            CreateMap<User, RegisterViewModel>().ReverseMap();

            CreateMap<AllProductsViewModel, Product>().ReverseMap();

            CreateMap<ContactUsViewModel, ContactMessage>()
                .ForPath(e => e.UserName, opt => opt.MapFrom(src => src.Name))
                .ForPath(e => e.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<AllContactUsMessagesViewModel, ContactMessage>()
                .ForPath(e => e.UserName, opt => opt.MapFrom(src => src.SenderName))
                .ForPath(e => e.UserEmail, opt => opt.MapFrom(src => src.SenderEmail))
                .ForPath(e => e.SentOn, opt => opt.MapFrom(src => src.SentOn))
                .ReverseMap();
        }
    }
}