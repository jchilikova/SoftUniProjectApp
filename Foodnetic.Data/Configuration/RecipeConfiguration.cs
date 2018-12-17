using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasMany(r => r.Comments)
                .WithOne(x => x.Recipe)
                .HasForeignKey(x => x.RecipeId);

            builder.HasMany(r => r.Stars)
                .WithOne(x => x.Recipe)
                .HasForeignKey(x => x.RecipeId);
        }
    }
}
