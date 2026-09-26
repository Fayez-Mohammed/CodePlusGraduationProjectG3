using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class CourseConfigration:BaseEntityConfiguration<Course>
    {
        public override void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(c => c.Slug)
                .IsUnique();

            builder.Property(c => c.Price)
                .HasPrecision(18, 2);

            builder.Property(c => c.Level)
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
           
        }
    }
}
