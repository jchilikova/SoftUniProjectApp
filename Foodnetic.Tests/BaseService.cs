using System;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Foodnetic.App.Mapping;
using Foodnetic.Data;
using Foodnetic.Models;
using Foodnetic.Services;
using Foodnetic.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Foodnetic.Tests
{
    public class BaseService
    {
        protected IServiceProvider ServiceProvider { get; set; }

        protected FoodneticDbContext DbContext { get; set; }

        [SetUp]
        public void Init()
        {
            Mapper.Reset();
            Mapper.Initialize(x => { x.AddProfile<MappingProfile>(); });
            var services = SetServices();
            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<FoodneticDbContext>();
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
                opt => opt.UseInMemoryDatabase(Guid.NewGuid()
                    .ToString()));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IFridgeService, FridgeService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IRecipeService, RecipeService>();


            services.AddIdentity<FoodneticUser, IdentityRole>()
                .AddEntityFrameworkStores<FoodneticDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();

            return services;
        }
    }
}
