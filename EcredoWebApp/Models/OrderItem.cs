using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcredoWebApp.Models;

public class OrderItem
{
    [Key]
    public Guid OrderItemId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid OrderId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    [Column(TypeName = "numeric(12,2)")]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    // Navigation Properties
    public Order Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
}