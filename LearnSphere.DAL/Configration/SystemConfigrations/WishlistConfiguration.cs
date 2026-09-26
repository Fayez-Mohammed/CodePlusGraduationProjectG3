using LearnSphere.DAL.Configration.BaseConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public  void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            //builder.Ignore(w => w.Id);
            //builder.Ignore(w => w.CreatedAt);
            //builder.Ignore(w => w.UpdatedAt);
            //builder.Ignore(w => w.IsDeleted);

            builder.HasOne(w => w.Student)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Course)
                .WithMany(c => c.Wishlists)
                .HasForeignKey(w => w.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasKey(x => new {x.StudentId, x.CourseId});

        }
    }
}
