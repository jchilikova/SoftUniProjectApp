using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class RecipeMenuConfiguration : IEntityTypeConfiguration<RecipeMenu>
    {
        public void Configure(EntityTypeBuilder<RecipeMenu> builder)
        {
            builder.HasKey(rt => new {rt.RecipeId, rt.MenuId});

            builder.HasOne(x => x.Recipe)
                .WithMany(x => x.Menus)
                .HasForeignKey(x => x.RecipeId);

            builder.HasOne(x => x.Menu)
                .WithMany(x => x.RecipeMenus)
                .HasForeignKey(x => x.MenuId);
        }
    }
}
