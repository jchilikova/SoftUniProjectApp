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

        }
    }   
}
