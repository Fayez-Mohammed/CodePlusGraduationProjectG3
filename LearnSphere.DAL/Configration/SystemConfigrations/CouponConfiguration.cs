using LearnSphere.DAL.Configration.BaseConfigrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnSphere.DAL.Configration.SystemConfigrations
{
    public class CouponConfiguration : BaseEntityConfiguration<Coupon>
    {
        public override void Configure(EntityTypeBuilder<Coupon> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.DiscountType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.DiscountValue)
                .HasPrecision(18, 2);

        }
    }
}
