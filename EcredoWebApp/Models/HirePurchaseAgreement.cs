using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class HirePurchaseAgreement
{
    [Key]
    public Guid HirePurchaseAgreementId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid OrderId { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal Deposit { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal RemainingBalance { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0.01, double.MaxValue)]
    public decimal MonthlyInstallment { get; set; }

    [Range(1, 120)]
    public int DurationMonths { get; set; }

    public DateTime NextPaymentDate { get; set; }

    public AgreementStatus Status { get; set; } = AgreementStatus.Active;

    // Navigation Property
    public Order Order { get; set; } = null!;
}