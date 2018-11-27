using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasMany(r => r.Comment)
                .WithOne(x => x.Recipe)
                .HasForeignKey(x => x.RecipeId);
        }
    }
}
