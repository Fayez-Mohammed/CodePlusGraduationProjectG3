using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class LessonConfiguration : BaseEntityConfiguration<Lesson>
    {
        public override void Configure(EntityTypeBuilder<Lesson> builder)
        {
            base.Configure(builder);
            builder.Property(l => l.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(l => l.Type)
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.HasOne(x => x.Section).WithMany(x => x.Lessons)
                .HasForeignKey(x => x.SectionId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
