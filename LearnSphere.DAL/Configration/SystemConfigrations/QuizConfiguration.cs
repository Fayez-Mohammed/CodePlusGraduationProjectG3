using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class QuizConfiguration : BaseEntityConfiguration<Quiz>
    {
        public override void Configure(EntityTypeBuilder<Quiz> builder)
        {
            base.Configure(builder);
            builder.Property(q => q.Title)
                       .IsRequired()
                       .HasMaxLength(200);

            builder.Property(q => q.PassingScore)
                .HasPrecision(5, 2);

            // One-to-One with Section
            builder.HasOne(q => q.Section)
                .WithOne(s => s.Quiz)
                .HasForeignKey<Quiz>(q => q.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
