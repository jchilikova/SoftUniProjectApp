using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class RecipeTagConfiguration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder.HasKey(rt => new {rt.RecipeId, rt.TagId});

            builder.HasOne(x => x.Recipe)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.RecipeId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.Recipes)
                .HasForeignKey(x => x.TagId);
        }
    }
}
