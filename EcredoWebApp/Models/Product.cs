using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcredoWebApp.Enums;

namespace EcredoWebApp.Models;

public class Product
{
    [Key]
    public Guid ProductId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(100)]
    public string? Model { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public ProductCondition Condition { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Category Category { get; set; } = null!;

    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}