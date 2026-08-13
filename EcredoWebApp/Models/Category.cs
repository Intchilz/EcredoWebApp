using System.ComponentModel.DataAnnotations;

namespace EcredoWebApp.Models;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}