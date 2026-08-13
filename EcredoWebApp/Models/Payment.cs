using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class Payment
{
    [Key]
    public Guid PaymentId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid OrderId { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public PaymentType PaymentType { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public Order Order { get; set; } = null!;
}