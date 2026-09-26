using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class LessonProgressConfiguration : BaseEntityConfiguration<LessonProgress>
    {
        public override void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            base.Configure(builder);
            builder.HasOne(lp => lp.Student)
                .WithMany(u => u.LessonProgresses)
                .HasForeignKey(lp => lp.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(l => l.Lesson).WithMany(lp => lp.LessonProgresses)
                .HasForeignKey(lp => lp.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
