using Foodnetic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodnetic.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Comments)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            builder.HasOne(u => u.DailyMenu)
                .WithOne(x => x.User)
                .HasForeignKey<Menu>(x => x.UserId);

            builder.HasOne(x => x.VirtualFridge)
                .WithOne(x => x.Owner)
                .HasForeignKey<VirtualFridge>(x => x.OwnerId);

        }
    }   
}
