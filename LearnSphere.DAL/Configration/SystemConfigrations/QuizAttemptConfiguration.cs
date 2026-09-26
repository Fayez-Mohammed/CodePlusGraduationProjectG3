using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class QuizAttemptConfiguration : BaseEntityConfiguration<QuizAttempt>
    {
        public override void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            base.Configure(builder);
            builder.Property(qa => qa.ScoreAchieved)
                .HasPrecision(5, 2);

            builder.HasOne(qa => qa.Student)
                .WithMany(u => u.QuizAttempts)
                .HasForeignKey(qa => qa.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(qa => qa.Quiz)
                .WithMany(q => q.QuizAttempts)
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
