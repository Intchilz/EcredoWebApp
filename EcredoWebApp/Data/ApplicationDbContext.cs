using EcredoWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcredoWebApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    // =========================================================
    // PRODUCT CATALOG
    // =========================================================

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();


    // =========================================================
    // ORDERS AND PAYMENTS
    // =========================================================

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();


    // =========================================================
    // CUSTOMER SERVICES
    // =========================================================

    public DbSet<SwapRequest> SwapRequests => Set<SwapRequest>();


    // =========================================================
    // FINANCING
    // =========================================================

    public DbSet<LaybyAgreement> LaybyAgreements => Set<LaybyAgreement>();
    public DbSet<HirePurchaseAgreement> HirePurchaseAgreements => Set<HirePurchaseAgreement>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        // =========================================================
        // CATEGORY
        // =========================================================

        builder.Entity<Category>()
            .HasKey(c => c.CategoryId);

        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        builder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Entity<Category>()
            .Property(c => c.Description)
            .HasMaxLength(500);


        // =========================================================
        // PRODUCT
        // =========================================================

        builder.Entity<Product>()
            .HasKey(p => p.ProductId);

        builder.Entity<Product>()
            .HasIndex(p => p.Name);

        builder.Entity<Product>()
            .HasIndex(p => p.Brand);

        builder.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Product>()
            .Property(p => p.Brand)
            .HasMaxLength(100);

        builder.Entity<Product>()
            .Property(p => p.Model)
            .HasMaxLength(100);

        builder.Entity<Product>()
            .Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Entity<Product>()
            .Property(p => p.Condition)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(12, 2);

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================================================
        // PRODUCT IMAGE
        // =========================================================

        builder.Entity<ProductImage>()
            .HasKey(pi => pi.ProductImageId);

        builder.Entity<ProductImage>()
            .Property(pi => pi.ImageUrl)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);


        // =========================================================
        // ORDER
        // =========================================================

        builder.Entity<Order>()
            .HasKey(o => o.OrderId);

        builder.Entity<Order>()
            .Property(o => o.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(12, 2);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================================================
        // ORDER ITEM
        // =========================================================

        builder.Entity<OrderItem>()
            .HasKey(oi => oi.OrderItemId);

        builder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasPrecision(12, 2);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================================================
        // PAYMENT
        // =========================================================

        builder.Entity<Payment>()
            .HasKey(p => p.PaymentId);

        builder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(12, 2);

        // PaymentType and PaymentStatus are enums.
        // EF Core will store them as integer values.

        builder.Entity<Payment>()
            .Property(p => p.PaymentType)
            .HasConversion<int>()
            .IsRequired();

        builder.Entity<Payment>()
            .Property(p => p.PaymentStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);


        // =========================================================
        // SWAP REQUEST
        // =========================================================

        builder.Entity<SwapRequest>()
            .HasKey(sr => sr.SwapRequestId);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.CurrentDeviceBrand)
            .HasMaxLength(100);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.CurrentDeviceModel)
            .HasMaxLength(100);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.DeviceCondition)
            .HasMaxLength(500);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.FaultDescription)
            .HasMaxLength(2000);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.DesiredDevice)
            .HasMaxLength(200);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.EstimatedTopUpAmount)
            .HasPrecision(12, 2);

        builder.Entity<SwapRequest>()
            .Property(sr => sr.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<SwapRequest>()
            .HasOne(sr => sr.User)
            .WithMany(u => u.SwapRequests)
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // =========================================================
        // LAYBY AGREEMENT
        // =========================================================

        builder.Entity<LaybyAgreement>()
            .HasKey(la => la.LaybyAgreementId);

        builder.Entity<LaybyAgreement>()
            .Property(la => la.Deposit)
            .HasPrecision(12, 2);

        builder.Entity<LaybyAgreement>()
            .Property(la => la.RemainingBalance)
            .HasPrecision(12, 2);

        builder.Entity<LaybyAgreement>()
            .Property(la => la.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<LaybyAgreement>()
            .HasOne(la => la.Order)
            .WithOne(o => o.LaybyAgreement)
            .HasForeignKey<LaybyAgreement>(la => la.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        // =========================================================
        // HIRE PURCHASE AGREEMENT
        // =========================================================

        builder.Entity<HirePurchaseAgreement>()
            .HasKey(hp => hp.HirePurchaseAgreementId);

        builder.Entity<HirePurchaseAgreement>()
            .Property(hp => hp.Deposit)
            .HasPrecision(12, 2);

        builder.Entity<HirePurchaseAgreement>()
            .Property(hp => hp.RemainingBalance)
            .HasPrecision(12, 2);

        builder.Entity<HirePurchaseAgreement>()
            .Property(hp => hp.MonthlyInstallment)
            .HasPrecision(12, 2);

        builder.Entity<HirePurchaseAgreement>()
            .Property(hp => hp.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Entity<HirePurchaseAgreement>()
            .HasOne(hp => hp.Order)
            .WithOne(o => o.HirePurchaseAgreement)
            .HasForeignKey<HirePurchaseAgreement>(hp => hp.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        // =========================================================
        // REPORT
        // =========================================================

        builder.Entity<Report>()
            .HasKey(r => r.ReportId);

        builder.Entity<Report>()
            .Property(r => r.TotalSales)
            .HasPrecision(12, 2);

        builder.Entity<Report>()
            .Property(r => r.OutstandingPayments)
            .HasPrecision(12, 2);
    }
}