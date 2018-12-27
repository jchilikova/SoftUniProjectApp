using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<FoodneticUser>
    {
        public void Configure(EntityTypeBuilder<FoodneticUser> builder)
        {
            builder.HasMany(u => u.Comments)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            builder.HasMany(u => u.DailyMenus)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.VirtualFridge)
                .WithOne(x => x.Owner)
                .HasForeignKey<VirtualFridge>(x => x.OwnerId);

        }
    }   
}
