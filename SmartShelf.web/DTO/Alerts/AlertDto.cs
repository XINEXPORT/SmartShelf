namespace SmartShelf.web.DTOs.Dashboard
{
    public class AlertDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        // (LowStock, OutOfStock)
        public string Type { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;

        public string Timestamp { get; set; } = string.Empty;
    }
}