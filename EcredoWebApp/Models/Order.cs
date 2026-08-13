using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class Order
{
    [Key]
    public Guid OrderId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentType PaymentType { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public LaybyAgreement? LaybyAgreement { get; set; }

    public HirePurchaseAgreement? HirePurchaseAgreement { get; set; }
}