using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class SwapRequest
{
    [Key]
    public Guid SwapRequestId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string CurrentDeviceBrand { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CurrentDeviceModel { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string DeviceCondition { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? FaultDescription { get; set; }

    [Required]
    [StringLength(200)]
    public string DesiredDevice { get; set; } = string.Empty;

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal EstimatedTopUpAmount { get; set; }

    [StringLength(1000)]
    public string? DeviceImage1 { get; set; }

    [StringLength(1000)]
    public string? DeviceImage2 { get; set; }

    [StringLength(1000)]
    public string? DeviceImage3 { get; set; }

    public SwapRequestStatus Status { get; set; } = SwapRequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public ApplicationUser User { get; set; } = null!;
}