using LearnSphere.DAL.Configration.BaseConfigrations;
using LearnSphere.DAL.Models.SystemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class QuestionOptionConfiguration : BaseEntityConfiguration<QuestionOption>
    {
        public override void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            base.Configure(builder);
            builder.Property(qo => qo.OptionText)
                 .IsRequired();

            builder.HasOne(qo => qo.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
