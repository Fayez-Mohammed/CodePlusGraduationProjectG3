using LearnSphere.DAL.Configration.BaseConfigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public  void Configure(EntityTypeBuilder<Payment> builder)
        {
            //builder.Ignore(w => w.Id);
            //builder.Ignore(w => w.CreatedAt);
            //builder.Ignore(w => w.UpdatedAt);
            //builder.Ignore(w => w.IsDeleted);

            builder.HasKey(p => p.TransactionId);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(p => p.Student)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Course)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne(p => p.Coupon)
                .WithMany()
                .HasForeignKey(p => p.CouponId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
