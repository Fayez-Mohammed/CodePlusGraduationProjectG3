using LearnSphere.DAL.Configration.BaseConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class CertificateConfiguration : BaseEntityConfiguration<Certificate>
    {
        public override void Configure(EntityTypeBuilder<Certificate> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.CertificateUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(c => c.Student)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Course)
                .WithMany(cr => cr.Certificates)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
