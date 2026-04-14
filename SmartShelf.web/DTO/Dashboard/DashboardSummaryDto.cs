namespace SmartShelf.web.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}