using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;
using SmartShelf.web.DTOs.Dashboard;
using SmartShelf.web.Interfaces;

namespace SmartShelf.web.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly SmartShelfContext _context;

        public InventoryService(SmartShelfContext context)
        {
            _context = context;
        }

        public List<InventoryItemDto> GetInventory()
        {
            return _context.TagCurrentState
                .Include(tcs => tcs.Tag)
                    .ThenInclude(tag => tag.Product)
                .Where(tcs => tcs.IsPresent)
                .GroupBy(tcs => new
                {
                    ProductId = tcs.Tag.Product.Id,
                    ProductName = tcs.Tag.Product.Name,
                    ImagePath = tcs.Tag.Product.ImagePath,
                    Threshold = tcs.Tag.Product.Threshold
                })
                .Select(g => new InventoryItemDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ImagePath = g.Key.ImagePath,
                    Count = g.Count(),
                    Threshold = g.Key.Threshold,
                    IsOutOfStock = g.Count() == 0,
                    IsLowStock = g.Count() > 0 && g.Count() < g.Key.Threshold,
                    StatusText = g.Count() == 0
                        ? "Out of stock"
                        : g.Count() < g.Key.Threshold
                            ? $"Low stock ({g.Count()} remaining)"
                            : "In stock"
                })
                .ToList();
        }
    }
}