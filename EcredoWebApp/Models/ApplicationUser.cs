using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EcredoWebApp.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Order> Orders { get; set; } = [];

    public ICollection<SwapRequest> SwapRequests { get; set; } = [];
}