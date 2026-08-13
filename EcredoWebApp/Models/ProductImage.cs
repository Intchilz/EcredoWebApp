using System.ComponentModel.DataAnnotations;

namespace EcredoWebApp.Models;

public class ProductImage
{
    [Key]
    public Guid ProductImageId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public Product Product { get; set; } = null!;
}