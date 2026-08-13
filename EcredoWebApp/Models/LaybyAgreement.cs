using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class LaybyAgreement
{
    [Key]
    public Guid LaybyAgreementId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid OrderId { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal Deposit { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal RemainingBalance { get; set; }

    public DateTime ExpiryDate { get; set; }

    public AgreementStatus Status { get; set; } = AgreementStatus.Active;

    // Navigation Property
    public Order Order { get; set; } = null!;
}