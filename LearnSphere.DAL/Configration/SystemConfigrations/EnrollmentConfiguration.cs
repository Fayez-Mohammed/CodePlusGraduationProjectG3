using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class EnrollmentConfiguration : BaseEntityConfiguration<Enrollment>
    {
        public override void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.CompletionPercentage)
                    .HasPrecision(5, 2);
            builder.HasOne(x => x.Course).WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Student).WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
           
        }
    }
}
