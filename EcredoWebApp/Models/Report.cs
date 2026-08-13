namespace EcredoWebApp.Models;

public class Report
{
    public Guid ReportId { get; set; } = Guid.NewGuid();

    public DateTime ReportMonth { get; set; }

    public decimal TotalSales { get; set; }

    public int TotalOrders { get; set; }

    public int NewCustomers { get; set; }

    public int CompletedSwaps { get; set; }

    public decimal OutstandingPayments { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}