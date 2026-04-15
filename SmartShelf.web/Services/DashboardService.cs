using SmartShelf.web.Data;
using Microsoft.EntityFrameworkCore;
using SmartShelf.web.DTOs.Dashboard;


namespace SmartShelf.web.Services
{
    public class DashboardService
    {
        private readonly SmartShelfContext _context;

        public DashboardService(SmartShelfContext context)
        {
            _context = context;
        }

        public DashboardDto GetDashboard()
        {
            // Get all products
            var products = _context.Product.ToList();

            // list all tags marked as present
            var presentTagStates = _context.TagCurrentState
                .Include(t => t.Tag)
                .Where(t => t.IsPresent)
                .ToList();

            // Count all the present tags for each product
            var inventoryData = products.Select(p =>
            {
                var count = presentTagStates.Count(t => t.Tag.ProductId == p.Id);

                return new
                {
                    Count = count,
                    Threshold = p.Threshold
                };
            }).ToList();

            // Build the dashboard 
            var dashboard = new DashboardDto
            {
                Summary = new DashboardSummaryDto
                {
                    TotalProducts = products.Count,
                    LowStockProducts = inventoryData.Count(i => i.Count > 0 && i.Count < i.Threshold),
                    OutOfStockProducts = inventoryData.Count(i => i.Count == 0),
                    LastUpdated = DateTime.UtcNow
                },
                Inventory = new List<InventoryItemDto>(),
                Alerts = new List<AlertDto>(),
                Status = new DashboardStatusDto()
            };

            return dashboard;
        }
    }
}