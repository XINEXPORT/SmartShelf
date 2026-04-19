using SmartShelf.web.Data;
using Microsoft.EntityFrameworkCore;
using SmartShelf.web.DTOs.Dashboard;
using SmartShelf.web.Interfaces;

namespace SmartShelf.web.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly SmartShelfContext _context;

        public SummaryService(SmartShelfContext context)
        {
            _context = context;
        }

        public DashboardSummaryDto GetSummary()
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

            // Build the summary 
            var summary = new DashboardSummaryDto
            {
                TotalProducts = products.Count,
                LowStockProducts = inventoryData.Count(i => i.Count > 0 && i.Count < i.Threshold),
                OutOfStockProducts = inventoryData.Count(i => i.Count == 0),
                LastUpdated = DateTime.UtcNow
            };
            return summary ;
        }


    }
}
