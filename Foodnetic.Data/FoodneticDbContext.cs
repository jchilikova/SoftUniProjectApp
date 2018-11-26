using Foodnetic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foodnetic.Data
{
    public class FoodneticDbContext : IdentityDbContext
    {
        public FoodneticDbContext(DbContextOptions<FoodneticDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Grocery> Groceries { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Tag> Tags { get; set; }

    }
}
