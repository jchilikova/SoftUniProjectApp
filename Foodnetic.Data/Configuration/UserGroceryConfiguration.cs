using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class UserGroceryConfiguration : IEntityTypeConfiguration<UserGrocery>
    {
        public void Configure(EntityTypeBuilder<UserGrocery> builder)
        {
            builder.HasKey(ug => new {ug.UserId, ug.GroceryId});

            builder.HasOne(x => x.User)
                .WithMany(x => x.Groceries)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Grocery)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.GroceryId);
        }
    }
}
