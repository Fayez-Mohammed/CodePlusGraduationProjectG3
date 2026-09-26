using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.DAL.Models.SystemModels;

public class Payment//:BaseEntity
{
    public string TransactionId { get; set; } = null!; // Primary Key from Gateway
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!; // CreditCard, PayPal, etc.
    public PaymentStatus Status { get; set; } = PaymentStatus.Success;

    // Foreign Keys
    public string StudentId { get; set; } = null!;
    public string CourseId { get; set; } = null!;
    public string? CouponId { get; set; }=null!;

    // Navigation Properties
    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Coupon? Coupon { get; set; }
}