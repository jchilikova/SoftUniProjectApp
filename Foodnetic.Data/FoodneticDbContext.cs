using Foodnetic.Data.Configuration;
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

        public DbSet<RecipeMenu> RecipeMenus { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        public DbSet<RecipeTag> RecipeTags { get; set; }

        public DbSet<VirtualFridge> VirtualFridges { get; set; }

        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecipeMenuConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeTagConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }

}
