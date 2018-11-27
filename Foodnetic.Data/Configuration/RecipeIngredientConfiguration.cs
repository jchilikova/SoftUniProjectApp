using System;
using System.Collections.Generic;
using System.Text;
using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.HasKey(ri => new {ri.RecipeId, ri.IngredientId});

            builder.HasOne(x => x.Recipe)
                .WithMany(x => x.Ingredients)
                .HasForeignKey(x => x.RecipeId);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.Recipes)
                .HasForeignKey(x => x.IngredientId);
        }
    }
}
