using LearnSphere.DAL.Models.BaseModels;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = null!;
    public DiscountType DiscountType { get; set; } // Enum: Percentage, FixedAmount
    public decimal DiscountValue { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UsageLimit { get; set; } = 100;
    public int TimesUsed { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
