namespace SmartShelf.web.DTOs.Dashboard
{
    public class InventoryItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Count { get; set; }
        public int Threshold { get; set; }

        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }

        public string StatusText { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}