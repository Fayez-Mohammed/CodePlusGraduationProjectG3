using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class SectionConfigration:BaseEntityConfiguration<Section>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            base.Configure(builder);
            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasOne(x => x.Course).WithMany(x => x.Sections)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
