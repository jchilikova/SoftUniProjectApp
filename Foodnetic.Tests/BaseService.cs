using System;
using AutoMapper;
using Foodnetic.App.Mapping;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Services.Tests
{
    public class BaseService
    {
        protected IServiceProvider ServiceProvider { get; set; }

        protected FoodneticDbContext DbContext { get; set; }

        [SetUp]
        public void Init()
        {
            Mapper.Reset();

            Mapper.Initialize(x =>
            {
                x.AddProfile<MappingProfile>();
            });

            var services = SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<FoodneticDbContext>();

            var httpContext = this.ServiceProvider.GetService<IHttpContextAccessor>();
            httpContext.HttpContext.RequestServices = this.ServiceProvider.CreateScope().ServiceProvider;
        }

        [TearDown]
        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<FoodneticDbContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IFridgeService, FridgeService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IRecipeService, RecipeService>();


            services.AddIdentity<FoodneticUser, IdentityRole>(opt =>
                    {
                        opt.Password.RequireDigit = false;
                        opt.Password.RequireLowercase = false;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequireUppercase = false;
                        opt.Password.RequiredLength = 1;
                        opt.Password.RequiredUniqueChars = 0;
                    })
                .AddEntityFrameworkStores<FoodneticDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();

            var context = new DefaultHttpContext();

            services.AddSingleton<IHttpContextAccessor>(
                new HttpContextAccessor()
                {
                    HttpContext = context,
                });

            return services;
        }
    }
}
